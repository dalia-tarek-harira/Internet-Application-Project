import React, { useState } from 'react';
import './login.css';
import { FaUser, FaLock } from "react-icons/fa";

const Login = ({ onLogin, onRegisterClick }) => {
  const [credentials, setCredentials] = useState({
    username: '',
    password: '',
  });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');

  const handleChange = (e) => {
    setCredentials({ ...credentials, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setIsLoading(true);
  
    try {
      const response = await fetch('http://localhost:5248/api/account/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(credentials),
      });
  
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Login failed');
      }
  
      const result = await response.json();
      console.log('✅ result from login:', result);

      
      // تأكد من وجود token في الـ response
      if (!result.accessToken) {
        throw new Error('No access token received');
      }
  
      localStorage.setItem('accessToken', result.accessToken);
      localStorage.setItem('refreshToken', result.refreshToken || '');
      localStorage.setItem('userId', result.userId);
      localStorage.setItem('currentUser', JSON.stringify({
        username: credentials.username,
        role: result.role,
      }));
  
      onLogin(result.role);
    } catch (error) {
      setError(error.message || 'Server error. Please try again.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className='login-container'>
      <form onSubmit={handleSubmit}>
        <h1>Login</h1>
        {error && <div className="error-message">{error}</div>}
        <div className="input-box">
          <input 
            type="text" 
            name="username" 
            placeholder='Username' 
            required 
            value={credentials.username}
            onChange={handleChange}
          />
          <FaUser className='icon' />
        </div>
        <div className="input-box">
          <input 
            type="password" 
            name="password" 
            placeholder='Password' 
            required 
            value={credentials.password}
            onChange={handleChange}
          />
          <FaLock className='icon' />
        </div>
        
        <button type="submit" disabled={isLoading}>
          {isLoading ? 'Logging in...' : 'Login'}
        </button>
        
        <div className="register-link">
          <p>Don't have an account? <a href="#" onClick={onRegisterClick}>Register</a></p>
        </div>
      </form>
    </div>
  );
};

export default Login;

