import React, { useState } from 'react';
import { FaUser, FaEnvelope, FaLock, FaHome, FaPhone, FaBirthdayCake } from 'react-icons/fa';
import './Register.css';

const Register = ({ onLoginClick, onRegister, goToLogin, selectedRole }) => {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
    address: '',
    age: '',
    phone: '',
    role: selectedRole || 'Reader',  
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
  
    if (formData.password !== formData.confirmPassword) {
      alert('Passwords do not match!');
      return;
    }
  
    try {
    
      const url = `http://localhost:5248/api/account/register?role=${encodeURIComponent(formData.role)}`;
  
      const response = await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          username: formData.username,
          email: formData.email,
          password: formData.password,
          confirmPassword: formData.confirmPassword,
          address: formData.address,
          age: parseInt(formData.age),
          phone: formData.phone,
        }),
      });
  
      const result = await response.json();
  
      if (response.ok) {
       
        localStorage.setItem('accessToken', result.accessToken);
        localStorage.setItem('refreshToken', result.refreshToken);
        localStorage.setItem('currentUser', JSON.stringify({
          username: formData.username,
          role: formData.role
        }));
  
        alert('Registration successful!');
        onRegister(formData.role);
        goToLogin();
      } else {
        alert(result.message || 'Registration failed');
      }
    } catch (err) {
      console.error(err);
      alert('Server error');
    }
  };
  
  

  return (
    <div className="wrapper">
      <form onSubmit={handleSubmit}>
        <h1>Register</h1>

        <div className="box">
          <input 
            type="text" 
            name="username" 
            placeholder="Username" 
            required 
            onChange={handleChange}
            value={formData.username}
          />
          <FaUser className="icon" />
        </div>

        <div className="box">
          <input 
            type="password" 
            name="password" 
            placeholder="Password" 
            required 
            onChange={handleChange}
            value={formData.password}
          />
          <FaLock className="icon" />
        </div>

        <div className="box">
          <input 
            type="password" 
            name="confirmPassword" 
            placeholder="Confirm Password" 
            required 
            onChange={handleChange}
            value={formData.confirmPassword}
          />
          <FaLock className="icon" />
        </div>

        <div className="box">
          <input 
            type="email" 
            name="email" 
            placeholder="Email" 
            required 
            onChange={handleChange}
            value={formData.email}
          />
          <FaEnvelope className="icon" />
        </div>

        <div className="box">
          <input 
            type="text" 
            name="address" 
            placeholder="Address" 
            required 
            onChange={handleChange}
            value={formData.address}
          />
          <FaHome className="icon" />
        </div>

        <div className="box">
          <input 
            type="number" 
            name="age" 
            placeholder="Age" 
            required 
            onChange={handleChange}
            value={formData.age}
          />
          <FaBirthdayCake className="icon" />
        </div>

        <div className="box">
          <input 
            type="tel" 
            name="phone" 
            placeholder="Phone" 
            required 
            onChange={handleChange}
            value={formData.phone}
          />
          <FaPhone className="icon" />
        </div>

        <div className="Registerr">
          <p>
            Already have an account?{' '}
            <button type="button" onClick={onLoginClick}>
              Login
            </button>
          </p>
        </div>

        <button type="submit">Register</button>
      </form>
    </div>
  );
};

export default Register;




