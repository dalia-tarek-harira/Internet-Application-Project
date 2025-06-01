import React, { useState, useEffect } from 'react';
import './PendingAccounts.css';

const PendingAccounts = () => {
  const [pendingAccounts, setPendingAccounts] = useState([]);

  useEffect(() => {
    const fetchPendingAccounts = async () => {
      try {
        const response = await fetch('http://localhost:5248/api/manageaccountstatus/pending', {
          headers: {
            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
          }
        });

        if (!response.ok) throw new Error('Failed to fetch pending accounts');

        const data = await response.json();
        setPendingAccounts(data);
      } catch (error) {
        console.error('Error fetching pending accounts:', error);
      }
    };

    fetchPendingAccounts();
  }, []);

  const handleApprove = async (account) => {
    try {
      const response = await fetch(`http://localhost:5248/api/manageaccountstatus/accept/${account.id}`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
      });
  
      if (!response.ok) throw new Error('Failed to approve account');
  
      const updatedPending = pendingAccounts.filter(a => a.id !== account.id);
      setPendingAccounts(updatedPending);
    } catch (error) {
      console.error('Error approving account:', error);
    }
  };
  
  const handleReject = async (accountId) => {
    try {
      const response = await fetch(`http://localhost:5248/api/manageaccountstatus/reject/${accountId}`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
        }
      });
  
      if (!response.ok) throw new Error('Failed to reject account');
  
      const updatedPending = pendingAccounts.filter(a => a.id !== accountId);
      setPendingAccounts(updatedPending);
    } catch (error) {
      console.error('Error rejecting account:', error);
    }
  };
  

  return (
    <div className="pending-accounts">
      <h2>Pending Book Owner Accounts</h2>
      {pendingAccounts.length === 0 ? (
        <p>No pending accounts</p>
      ) : (
        <ul>
          {pendingAccounts.map((account, index) => (
            <li key={index}>
              <div>
                <p>Username: {account.userName}</p>
                <p>Email: {account.email}</p>
               
              </div>
              <div className="actions">
                <button onClick={() => handleApprove(account)}>Approve</button>
                <button onClick={() => handleReject(account.userName)}>Reject</button>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default PendingAccounts;
