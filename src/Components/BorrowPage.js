import React, { useState, useEffect } from 'react';
import { FaArrowLeft, FaCalendarAlt, FaSpinner, FaBook } from 'react-icons/fa';
import * as signalR from '@microsoft/signalr';
import './BorrowPage.css';

function BorrowPage({ bookId, onBack }) {
  const [formData, setFormData] = useState({
    BookPostId: bookId ? parseInt(bookId) : null,
    StartDate: '',
    EndDate: ''
  });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [notificationCount, setNotificationCount] = useState(0);  // Added to handle notifications

  // ✅ اتصال SignalR
  useEffect(() => {
    const userId = localStorage.getItem('userId'); // لازم تحط userId في localStorage بعد تسجيل الدخول

    if (!userId) {
      console.error('User ID not found in localStorage');
      return;
    }

    const connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5248/bookhub') // غيّر اللينك حسب حالتك
      .withAutomaticReconnect()
      .build();

    connection.start()
      .then(() => {
        console.log('Connected to SignalR Hub');

        connection.invoke('RegisterReader', userId)
          .then(() => console.log('Reader registered in group'))
          .catch(err => console.error('Error registering reader:', err));

        connection.on('RequestAccepted', (data) => {
          console.log('Notification received:', data);
          setNotificationCount(prev => prev + 1);
        });
        
      })
      .catch(err => console.error('Error connecting to SignalR Hub:', err));

    return () => {
      connection.stop();
    };
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!formData.BookPostId) {
      setError('Invalid book reference');
      return;
    }

    if (!formData.StartDate || !formData.EndDate) {
      setError('Please select both start and end dates');
      return;
    }

    if (new Date(formData.StartDate) >= new Date(formData.EndDate)) {
      setError('End date must be after the start date');
      return;
    }

    setIsSubmitting(true);
    setError('');
    setSuccess('');

    try {
      const data = new FormData();
      data.append('BookPostId', formData.BookPostId);
      data.append('StartDate', formData.StartDate);
      data.append('EndDate', formData.EndDate);

      const response = await fetch('http://localhost:5248/api/borrowrequest', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        },
        body: data
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || 'Borrow request failed');
      }

      const result = await response.json(); // امسك الريسبونس بتاع الطلب

      setSuccess('Borrow request submitted successfully! ✅');

      // ✅ نادِ BookAvailability باستخدام BorrowRequestID اللي رجع
      if (result && result.borrowRequestID) {
        console.log('Updating book status with borrowRequestId:', result.borrowRequestID);
        await updateBookStatus(result.borrowRequestID);
      } else {
        console.warn('No borrow request ID returned from the API');
      }

      setTimeout(() => onBack(), 2000);
    } catch (err) {
      console.error('Error:', err);
      setError(err.message || 'Failed to submit borrow request');
    } finally {
      setIsSubmitting(false);
    }
  };

  const updateBookStatus = async (borrowRequestId) => {
    try {
      const response = await fetch(`http://localhost:5248/api/BookPosts/Bookavailability?id=${borrowRequestId}`, {
        method: 'PUT',
        headers: {
          Authorization: `Bearer ${localStorage.getItem('accessToken')}`
        }
      });

      if (!response.ok) {
        const errorText = await response.text();
        console.error('Failed to update book status:', errorText);
      } else {
        const updatedBook = await response.json();
        console.log('Book status updated:', updatedBook);
      }
    } catch (error) {
      console.error('Error updating book status:', error);
    }
  };

  return (
    <div className="borrow-page">
      <button onClick={onBack} className="back-button">
        <FaArrowLeft /> Back
      </button>

      <div className="book-details">
        <h2>Borrow Book (ID: {bookId || 'N/A'})</h2>
      </div>

      <form onSubmit={handleSubmit} className="borrow-form">
        {error && <div className="error-message">{error}</div>}
        {success && <div className="success-message">{success}</div>}

        <div className="form-group">
          <label><FaCalendarAlt /> Start Date</label>
          <input
            type="date"
            name="StartDate"
            value={formData.StartDate}
            onChange={handleChange}
            min={new Date().toISOString().split('T')[0]}
            required
          />
        </div>

        <div className="form-group">
          <label><FaCalendarAlt /> End Date</label>
          <input
            type="date"
            name="EndDate"
            value={formData.EndDate}
            onChange={handleChange}
            min={formData.StartDate || new Date().toISOString().split('T')[0]}
            required
          />
        </div>

        <button
          type="submit"
          className="submit-button"
          disabled={isSubmitting}
        >
          {isSubmitting ? (
            <><FaSpinner className="spinner-icon" /> Processing...</>
          ) : (
            <><FaBook /> Confirm Borrow</>
          )}
        </button>
      </form>
    </div>
  );
}

export default BorrowPage;








