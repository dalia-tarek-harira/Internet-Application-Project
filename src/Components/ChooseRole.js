import React from 'react';
import './ChooseRole.css';

function ChooseRole({ onRoleSelect }) {
  const handleSelect = (role) => {
    if (onRoleSelect) {
      onRoleSelect(role);
    }
  };

  return (
    <div className="choose-role-wrapper">
      <h2>Select Your Role</h2>
      <div className="role-buttons">
        <button onClick={() => handleSelect('Reader')}>Reader</button>
        <button onClick={() => handleSelect('Book Owner')}>Book Owner</button>
        <button onClick={() => handleSelect('Admin')}>Admin</button>
      </div>
    </div>
  );
}

export default ChooseRole;

