import React, { useState } from 'react';
import './GetUsers.css';

function GetUsers() {
  const [username, setUsername] = useState('');
  const [userProfile, setUserProfile] = useState(null);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const fetchUserProfile = async () => {
    if (!username.trim()) {
      setError('Please enter a username');
      return;
    }

    setLoading(true);
    setError('');
    try {
      const token = localStorage.getItem('accessToken');
      if (!token) throw new Error('Please login first');

      const response = await fetch(`http://localhost:5248/api/account/${encodeURIComponent(username)}`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || 'Failed to fetch user profile');
      }

      const data = await response.json();
      console.log('API Response:', data);

     
      const normalizedData = {};
      Object.keys(data).forEach(key => {
        normalizedData[key.toLowerCase()] = data[key];
      });

      if (!normalizedData.username) {
        throw new Error('User data is incomplete');
      }

      setUserProfile({
        username: normalizedData.username,
        phone: normalizedData.phone || 'Not available',
        address: normalizedData.address || 'Not available'
      });
    } catch (err) {
      console.error('Error:', err);
      setError(err.message);
      setUserProfile(null);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="getusers-container">
      <div className="getusers-form">
        <h2>User Profile Search</h2>

        <div className="input-group">
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            placeholder="Enter username"
            className="getusers-input"
          />
          <button 
            onClick={fetchUserProfile} 
            disabled={loading || !username.trim()}
            className="getusers-button"
          >
            {loading ? 'Searching...' : 'Search'}
          </button>
        </div>

        {error && <div className="error-message">{error}</div>}

        {userProfile && (
          <div className="profile-card">
            <h3>Profile Information</h3>
            <div className="profile-field">
              <span className="field-label">Username:</span>
              <span className="field-value">{userProfile.username}</span>
            </div>
            <div className="profile-field">
              <span className="field-label">Phone:</span>
              <span className="field-value">{userProfile.phone}</span>
            </div>
            <div className="profile-field">
              <span className="field-label">Address:</span>
              <span className="field-value">{userProfile.address}</span>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

export default GetUsers;
