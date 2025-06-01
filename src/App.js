import React, { useState, useEffect } from 'react';
import './App.css';
import Login from './Components/Login';
import Register from './Components/Register';
import HomePage from './Components/HomePage';
import UploadPost from './Components/UploadPost';
import ChooseRole from './Components/ChooseRole';
import GetUsers from './Components/GetUsers';
import BorrowPage from './Components/BorrowPage';
import PendingAccounts from './Components/PendingAccounts';
import PendingPosts from './Components/PendingPosts';
import { FaSearch, FaTimes, FaHome, FaUserCog, FaUsers, FaClipboardList, FaSignInAlt, FaUserPlus, FaSignOutAlt, FaUpload, FaClipboardCheck,FaBell } from 'react-icons/fa';
import ManagePosts from './Components/ManagePosts';
import BorrowRequests from './Components/BorrowRequests';

function App() {
  const [currentPage, setCurrentPage] = useState('home');
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [userRole, setUserRole] = useState(null);
  const [selectedRole, setSelectedRole] = useState(null);
  const [borrowBookId, setBorrowBookId] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [searchType, setSearchType] = useState('title');
  const [showSearch, setShowSearch] = useState(false);
  const [notificationCount, setNotificationCount] = useState(0); // Adding notification count state
  const [userId, setUserId] = useState(localStorage.getItem('userId'));

  useEffect(() => {
    const storedUser = JSON.parse(localStorage.getItem('currentUser'));
    if (storedUser) {
      setIsLoggedIn(true);
      setUserRole(storedUser.role);
      setUserId(localStorage.getItem('userId')); // ← تأكد إن userId متخزن
    }
  }, []);
  

  const handleLogin = (role) => {
    setIsLoggedIn(true);
    setUserRole(role);
    localStorage.setItem('currentUser', JSON.stringify({ role }));
    setCurrentPage('home');
  };

  const handleRegister = (role) => {
    setIsLoggedIn(true);
    setUserRole(role);
    localStorage.setItem('currentUser', JSON.stringify({ role }));
    setCurrentPage('home');
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
    setUserRole(null);
    localStorage.removeItem('currentUser');
    setCurrentPage('home');
    setSearchTerm('');
    setShowSearch(false);
  };

  const handleBorrowClick = (bookId) => {
    setBorrowBookId(bookId);
    setCurrentPage('borrow');
  };

  const handleBackFromBorrow = () => {
    setCurrentPage('home');
  };

  const handleSearchClick = (type) => {
    setSearchType(type);
    setShowSearch(true);
    setSearchTerm('');
  };

  const closeSearch = () => {
    setShowSearch(false);
    setSearchTerm('');
  };

  const handleSearchChange = (e) => {
    setSearchTerm(e.target.value);
  };

  const handleHomeClick = () => {
    setCurrentPage('home');
    setSearchTerm('');
    setShowSearch(false);
  };

  return (
    <div className="App">
      <nav className="navbar">
        <div className="logo">BookApp</div>
        <div className="nav-links">
          {/* Home Button */}
          <button onClick={handleHomeClick} className="nav-button">
            <FaHome className="nav-icon" />
            <span>Home</span>
          </button>

          {/* Search Section */}
          <div className="search-section">
            <button onClick={() => handleSearchClick('title')} className="nav-button search-button">
              <FaSearch className="nav-icon" />
              <span>Search</span>
            </button>

            {showSearch && (
              <div className="search-box">
                <div className="search-options">
                  <select
                    value={searchType}
                    onChange={(e) => setSearchType(e.target.value)}
                    className="search-select"
                  >
                    <option value="title">By Title</option>
                    <option value="price">By Price</option>
                  </select>
                </div>
                <input
                  type={searchType === 'price' ? 'number' : 'text'}
                  placeholder={searchType === 'price' ? 'Enter max price...' : 'Search by title...'}
                  value={searchTerm}
                  onChange={handleSearchChange}
                  autoFocus
                />
                <button className="close-btn" onClick={closeSearch}>
                  <FaTimes />
                </button>
              </div>
            )}
          </div>

          {/* Book Owner Menu */}
          {isLoggedIn && userRole === 'Book Owner' && (
            <>
              <button onClick={() => setCurrentPage('managePosts')} className="nav-button">
                <FaClipboardList className="nav-icon" />
                <span>Manage Posts</span>
              </button>

              <button onClick={() => setCurrentPage('upload')} className="nav-button">
                <FaUpload className="nav-icon" />
                <span>Upload Post</span>
              </button>

              <button onClick={() => setCurrentPage('borrowRequests')} className="nav-button">
                <FaClipboardCheck className="nav-icon" />
                <span>Borrow Requests</span>
              </button>
            </>
          )}

          {/* Admin Menu */}
          {isLoggedIn && userRole === 'Admin' && (
            <>
              <button onClick={() => setCurrentPage('pendingAccounts')} className="nav-button">
                <FaUserCog className="nav-icon" />
                <span>Pending Accounts</span>
              </button>

              <button onClick={() => setCurrentPage('pendingPosts')} className="nav-button">
                <FaClipboardList className="nav-icon" />
                <span>Pending Posts</span>
              </button>

              <button onClick={() => setCurrentPage('getUsers')} className="nav-button">
                <FaUsers className="nav-icon" />
                <span>Manage Users</span>
              </button>
            </>
          )}

          {/* Auth Buttons */}
          <div className="auth-buttons">
            {!isLoggedIn ? (
              <>
                <button onClick={() => setCurrentPage('login')} className="nav-button">
                  <FaSignInAlt className="nav-icon" />
                  <span>Login</span>
                </button>

                <button onClick={() => setCurrentPage('chooseRole')} className="nav-button">
                  <FaUserPlus className="nav-icon" />
                  <span>Register</span>
                </button>
              </>
            ) : (
              <button onClick={handleLogout} className="nav-button">
                <FaSignOutAlt className="nav-icon" />
                <span>Logout</span>
              </button>
            )}
          </div>
        </div>
      </nav>

      <div className="content-container">
      {currentPage === 'home' && (
  <HomePage
    isLoggedIn={isLoggedIn}
    userRole={userRole}
    userId={userId} // ⬅️ أضف دي
    onBorrowClick={handleBorrowClick}
    searchTerm={searchTerm}
    searchType={searchType}
  />
)}

        {currentPage === 'login' && (
          <Login onLogin={handleLogin} onRegisterClick={() => setCurrentPage('chooseRole')} />
        )}
        {currentPage === 'chooseRole' && (
          <ChooseRole
            onRoleSelect={(role) => {
              setSelectedRole(role);
              setCurrentPage('register');
            }}
          />
        )}
        {currentPage === 'register' && (
          <Register
            selectedRole={selectedRole}
            onRegister={handleRegister}
            goToLogin={() => setCurrentPage('login')}
          />
        )}
        {currentPage === 'upload' && <UploadPost />}
        {currentPage === 'getUsers' && <GetUsers />}
        {currentPage === 'borrow' && <BorrowPage bookId={borrowBookId} onBack={handleBackFromBorrow} />}
        {currentPage === 'pendingAccounts' && <PendingAccounts />}
        {currentPage === 'pendingPosts' && <PendingPosts />}
        {currentPage === 'managePosts' && <ManagePosts />}
        {currentPage === 'borrowRequests' && <BorrowRequests />}
      </div>
    </div>
  );
}

export default App;










