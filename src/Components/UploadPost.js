import React, { useState } from 'react';
import './UploadPost.css';

const UploadPost = () => {
  const [bookName, setBookName] = useState('');
  const [authorName, setAuthorName] = useState('');
  const [bookPrice, setBookPrice] = useState('');
  const [bookImage, setBookImage] = useState(null);
  const [preview, setPreview] = useState(null);
  const [isbn, setIsbn] = useState('');
  const [language, setLanguage] = useState('');
  const [genre, setGenre] = useState('');
  const [startAvailabilityDate, setStartAvailabilityDate] = useState('');
  const [endAvailabilityDate, setEndAvailabilityDate] = useState('');
  const [bookOwnerName, setBookOwnerName] = useState(''); // ✅ جديد

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    setBookImage(file);
    setPreview(URL.createObjectURL(file));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const token = localStorage.getItem('accessToken');

    if (!token) {
      alert('You must be logged in to upload a book.');
      return;
    }

    const formData = new FormData();
    formData.append('Title', bookName);
    formData.append('language', language);
    formData.append('genere', genre);
    formData.append('ISBN', isbn);
    formData.append('BorrowPrice', bookPrice);
    formData.append('StartAvailability', startAvailabilityDate);
    formData.append('EndAvailability', endAvailabilityDate);
    formData.append('CoverImage', bookImage);
    formData.append('bookownername', bookOwnerName); // ✅ جديد

    try {
      const response = await fetch('http://localhost:5248/api/bookposts/add', {
        method: 'POST',
        headers: {
          Authorization: `Bearer ${token}`,
        },
        body: formData,
      });

      if (response.ok) {
        alert('Book post uploaded successfully!');
        // reset all fields
        setBookName('');
        setAuthorName('');
        setBookPrice('');
        setIsbn('');
        setLanguage('');
        setGenre('');
        setStartAvailabilityDate('');
        setEndAvailabilityDate('');
        setBookImage(null);
        setPreview(null);
        setBookOwnerName(''); // ✅ جديد
      } else {
        const result = await response.json();
        alert(result.message || 'Failed to upload post.');
      }
    } catch (error) {
      console.error('Error uploading post:', error);
      alert('Something went wrong. Please try again later.');
    }
  };

  return (
    <div className="upload-post-container">
      <h2>Upload a Book Post</h2>
      <form onSubmit={handleSubmit} className="upload-form">
        <input
          type="text"
          placeholder="Your Name"
          value={bookOwnerName}
          onChange={(e) => setBookOwnerName(e.target.value)}
          required
        />
        <input
          type="text"
          placeholder="Book Title"
          value={bookName}
          onChange={(e) => setBookName(e.target.value)}
          required
        />
        <input
          type="text"
          placeholder="Language"
          value={language}
          onChange={(e) => setLanguage(e.target.value)}
          required
        />
        <input
          type="text"
          placeholder="Genre"
          value={genre}
          onChange={(e) => setGenre(e.target.value)}
          required
        />
        <input
          type="text"
          placeholder="ISBN"
          value={isbn}
          onChange={(e) => setIsbn(e.target.value)}
          required
        />
        <input
          type="number"
          placeholder="Price"
          value={bookPrice}
          onChange={(e) => setBookPrice(e.target.value)}
          required
        />
        <div className="date-range">
          <div className="date-field-inline">
            <label>Start Avail:</label>
            <input
              type="date"
              value={startAvailabilityDate}
              onChange={(e) => setStartAvailabilityDate(e.target.value)}
              required
            />
          </div>
          <div className="date-field-inline">
            <label>End Avail:</label>
            <input
              type="date"
              value={endAvailabilityDate}
              onChange={(e) => setEndAvailabilityDate(e.target.value)}
              required
            />
          </div>
        </div>

        <input
          type="file"
          accept="image/*"
          onChange={handleImageChange}
          required
        />
        {preview && <img src={preview} alt="Preview" className="preview-img" />}
        <button type="submit">Upload</button>
      </form>
    </div>
  );
};

export default UploadPost;



 