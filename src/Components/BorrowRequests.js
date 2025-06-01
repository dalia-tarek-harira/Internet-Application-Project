import React, { useState, useEffect } from 'react';
import './BorrowRequests.css';

function BorrowRequests() {
  const [requests, setRequests] = useState([]);

  const accessToken = localStorage.getItem('accessToken');

  // دالة لجلب الطلبات
  const fetchUserRequests = () => {
    fetch('http://localhost:5248/api/borrowRequest/AllRequests', {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${accessToken}`,
      }
    })
      .then(response => {
        if (!response.ok) {
          throw new Error('Failed to fetch user requests');
        }
        return response.json();
      })
      .then(data => {
        console.log('User requests fetched successfully', data);
        setRequests(data);
      })
      .catch(error => {
        console.error('Error fetching requests', error);
      });
  };

  // استخدام useEffect لجلب الطلبات عند تحميل الصفحة
  useEffect(() => {
    fetchUserRequests();
  }, []);

  // دالة للتعامل مع الموافقة
  const handleApprove = (requestId) => {
    // تحديث الحالة في الواجهة الأمامية فورًا
    const updatedRequests = requests.map(request => {
      if (request.id === requestId) {
        return { ...request, status: 'Accepted' };
      }
      return request;
    });

    setRequests(updatedRequests); // تحديث الواجهة الأمامية

    // إرسال التحديث إلى الـ API
    fetch(`http://localhost:5248/api/borrowRequest/${requestId}/Accepted`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${accessToken}`,
      }
    })
      .then(response => {
        if (!response.ok) {
          throw new Error('Failed to approve request');
        }
        console.log('Request approved successfully');
        // إعادة جلب البيانات المحدثة من الـ API
        fetchUserRequests(); // جلب البيانات مرة أخرى
      })
      .catch(error => {
        console.error('Error approving request', error);
        
        // إذا حدث خطأ في الـ API، نعيد الحالة إلى ما كانت عليه
        const revertedRequests = requests.map(request => {
          if (request.id === requestId) {
            return { ...request, status: 'Pending' };
          }
          return request;
        });
        setRequests(revertedRequests); // إعادة الحالة
      });
  };

  // دالة للتعامل مع الرفض
  const handleReject = (requestId) => {
    // تحديث الحالة في الواجهة الأمامية فورًا
    const updatedRequests = requests.map(request => {
      if (request.id === requestId) {
        return { ...request, status: 'Rejected' };
      }
      return request;
    });

    setRequests(updatedRequests); // تحديث الواجهة الأمامية

    // إرسال التحديث إلى الـ API
    fetch(`http://localhost:5248/api/borrowRequest/${requestId}/Rejected`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${accessToken}`,
      }
    })
      .then(response => {
        if (!response.ok) {
          throw new Error('Failed to reject request');
        }
        console.log('Request rejected successfully');
        // إعادة جلب البيانات المحدثة من الـ API
        fetchUserRequests(); // جلب البيانات مرة أخرى
      })
      .catch(error => {
        console.error('Error rejecting request', error);

        // إذا حدث خطأ في الـ API، نعيد الحالة إلى ما كانت عليه
        const revertedRequests = requests.map(request => {
          if (request.id === requestId) {
            return { ...request, status: 'Pending' };
          }
          return request;
        });
        setRequests(revertedRequests); // إعادة الحالة
      });
  };

  return (
    <div className="borrow-requests-container">
      <h2>Borrow Requests</h2>

      {requests.length === 0 ? (
        <p>No borrow requests found.</p>
      ) : (
        <div className="requests-list">
          {requests.map((request, index) => (
            <div key={index} className="request-card">
              <h3>Book Name: {request.bookpost?.title || 'Unknown Title'}</h3>
              <p>Dates: {request.startDate} to {request.endDate}</p>
              <p>Status:
                <span className={`status-${request.status}`}>
                  {request.status}
                </span>
              </p>

              {request.status === 'Pending' && (
                <div className="request-actions">
                  <button
                    className="approve-btn"
                    onClick={() => handleApprove(request.id)}
                  >
                    Approve
                  </button>
                  <button
                    className="reject-btn"
                    onClick={() => handleReject(request.id)}
                  >
                    Reject
                  </button>
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default BorrowRequests;










