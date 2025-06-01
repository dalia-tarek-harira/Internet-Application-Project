import React, { useState, useEffect } from 'react';
import './PendingPosts.css';

const PendingPosts = () => {
  const [pendingPosts, setPendingPosts] = useState([]);

  useEffect(() => {
    const fetchPendingPosts = async () => {
      try {
        const token = localStorage.getItem('accessToken'); // جلب التوكن من localStorage

        const response = await fetch('http://localhost:5248/api/managebookposts/AllPeding', {
          headers: {
            'Authorization': `Bearer ${token}`, // إضافة التوكن في الهيدر
          },
        });

        if (!response.ok) throw new Error('Failed to fetch pending posts');

        const data = await response.json();
        setPendingPosts(data);
      } catch (error) {
        console.error('Error fetching pending posts:', error);
      }
    };

    fetchPendingPosts();
  }, []); // مفيش token في dependencies عشان نتجنب التحذير

  const handleApprove = async (postId) => {
    try {
      const token = localStorage.getItem('accessToken');

      const response = await fetch(`http://localhost:5248/api/managebookposts/Accept/${postId}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
      });

      if (!response.ok) throw new Error('Failed to approve post');

      setPendingPosts((prev) => prev.filter((post) => post.id !== postId));
    } catch (error) {
      console.error('Error approving post:', error);
    }
  };

  const handleReject = async (postId) => {
    try {
      const token = localStorage.getItem('accessToken');

      const response = await fetch(`http://localhost:5248/api/managebookposts/Reject/${postId}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
      });

      if (!response.ok) throw new Error('Failed to reject post');

      setPendingPosts((prev) => prev.filter((post) => post.id !== postId));
    } catch (error) {
      console.error('Error rejecting post:', error);
    }
  };

  return (
    <div className="pending-posts">
      <h2>Pending Book Posts</h2>
      {pendingPosts.length === 0 ? (
        <p>No pending posts</p>
      ) : (
        <ul>
          {pendingPosts.map((post) => (
            <li key={post.id}>
              <div>
                <h3>{post.title}</h3>
                <p>Author: {post.bookownername}</p>
                <p>Price: ${post.borrowPrice}</p>
                {post.preview && <img src={post.preview} alt="Book" className="post-preview" />}
              </div>
              <div className="actions">
                <button onClick={() => handleApprove(post.id)}>Approve</button>
                <button onClick={() => handleReject(post.id)}>Reject</button>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default PendingPosts;





