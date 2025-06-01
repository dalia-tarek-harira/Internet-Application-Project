import React, { useEffect, useState } from 'react';
import './ManagePosts.css';

const ManagePosts = () => {
  const [myPosts, setMyPosts] = useState([]);
  const [editPostId, setEditPostId] = useState(null);
  const [editData, setEditData] = useState({
    title: '',
    bookownername: '',
    borrowPrice: '',
    isbn: '',
    language: '',
    genre: '',
    description: '',
    startAvailability: '',
    endAvailability: '',
    coverImage: null,
    coverImageUrl: ''
  });
  const [loadedImages, setLoadedImages] = useState({});
  const [isLoading, setIsLoading] = useState(false);

  // دالة مساعدة لإصلاح مسار الصورة
  const getFullImageUrl = (relativePath) => {
    if (!relativePath) return '';
    if (relativePath.startsWith('http')) return relativePath;
    return `http://localhost:5248/${relativePath.replace(/^\/?uploads\//, 'uploads/')}`;
  };

  // جلب منشورات المستخدم
  useEffect(() => {
    const fetchMyPosts = async () => {
      setIsLoading(true);
      try {
        const token = localStorage.getItem('accessToken');
        if (!token) {
          alert('You need to log in!');
          setIsLoading(false);
          return;
        }

        const response = await fetch('http://localhost:5248/api/BookPosts/MyPosts', {
          headers: { Authorization: `Bearer ${token}` }
        });

        if (!response.ok) {
          console.error("Failed to fetch posts:", response.statusText);
          setIsLoading(false);
          return;
        }

        const data = await response.json();
        setMyPosts(data);
      } catch (error) {
        console.error("Error fetching posts:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchMyPosts();
  }, []);

  // حذف المنشور
  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this post?')) return;
    
    try {
      const token = localStorage.getItem('accessToken');
      if (!token) {
        alert('You need to log in!');
        return;
      }

      const response = await fetch(`http://localhost:5248/api/BookPosts/${id}`, {
        method: 'DELETE',
        headers: { Authorization: `Bearer ${token}` }
      });

      if (response.ok) {
        setMyPosts(prev => prev.filter(p => p.id !== id));
      } else {
        console.error("Failed to delete post:", await response.text());
      }
    } catch (error) {
      console.error("Error deleting post:", error);
    }
  };

  // بدء التعديل
  const handleEditClick = (post) => {
    setEditPostId(post.id);
    setEditData({
      title: post.title || '',
      bookownername: post.bookownername || '',
      borrowPrice: post.borrowPrice || '',
      isbn: post.isbn || '',
      language: post.language || '',
      genre: post.genre || '',
      description: post.description || '',
      startAvailability: post.startAvailability ? post.startAvailability.slice(0, 10) : '',
      endAvailability: post.endAvailability ? post.endAvailability.slice(0, 10) : '',
      coverImage: null,
      coverImageUrl: post.coverImageUrl || ''
    });
  };

  // تحديث بيانات التعديل
  const handleEditChange = (e) => {
    const { name, value, files } = e.target;
    setEditData(prev => ({
      ...prev,
      [name]: files ? files[0] : value
    }));
  };

  // حفظ التعديلات
  const handleSave = async () => {
    try {
      const token = localStorage.getItem('accessToken');
      if (!token) {
        alert('You need to log in!');
        return;
      }

      const formData = new FormData();
      formData.append('Title', editData.title || 'Untitled Book');
      formData.append('language', editData.language || 'English');
      formData.append('genere', editData.genre || 'General');
      formData.append('ISBN', editData.isbn || '');
      formData.append('BorrowPrice', editData.borrowPrice || 0);
      formData.append('bookownername', editData.bookownername || '');

      const startDate = editData.startAvailability || new Date().toISOString().split('T')[0];
      const endDate = editData.endAvailability || new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];
      formData.append('StartAvailability', startDate);
      formData.append('EndAvailability', endDate);

      if (editData.coverImage) {
        formData.append('CoverImage', editData.coverImage);
      } else if (editData.coverImageUrl) {
        formData.append('CoverImageUrl', editData.coverImageUrl);
      }

      const response = await fetch(`http://localhost:5248/api/BookPosts/${editPostId}`, {
        method: 'PUT',
        headers: { Authorization: `Bearer ${token}` },
        body: formData
      });

      if (!response.ok) {
        const errorData = await response.json();
        const errorMessages = Object.entries(errorData.errors || {})
          .map(([field, errors]) => `${field}: ${errors.join(', ')}`)
          .join('\n');
        alert(`Validation errors:\n${errorMessages}`);
        return;
      }

      // إعادة جلب البيانات بعد التحديث الناجح
      const updatedPost = await response.json();
      setMyPosts(prev => prev.map(p => p.id === editPostId ? updatedPost : p));
      setEditPostId(null);
    } catch (error) {
      console.error('Error updating post:', error);
    
    }
  };

  // إلغاء التعديل
  const handleCancel = () => {
    setEditPostId(null);
  };

  if (isLoading) {
    return <div className="loading">Loading posts...</div>;
  }

  return (
    <div className="manage-posts-container">
      <h2>My Posts</h2>
      {myPosts.length === 0 ? (
        <p>No posts found.</p>
      ) : (
        myPosts.map(post => (
          <div key={post.id} className="post-card">
            <div className="image-container">
              <img
                src={getFullImageUrl(post.coverImageUrl)}
                alt="book cover"
                className="post-image"
                style={{ display: loadedImages[post.id] ? 'block' : 'none' }}
                onLoad={() => setLoadedImages(prev => ({ ...prev, [post.id]: true }))}
                onError={(e) => {
                  e.target.src = 'https://via.placeholder.com/150?text=No+Cover';
                }}
              />
              {!loadedImages[post.id] && (
                <div className="image-placeholder">Loading image...</div>
              )}
            </div>
            <div className="post-info">
              {editPostId === post.id ? (
                <>
                  <input name="title" value={editData.title} onChange={handleEditChange} placeholder="Book Name" />
                  <input name="bookownername" value={editData.bookownername} onChange={handleEditChange} placeholder="Author Name" />
                  <input name="borrowPrice" value={editData.borrowPrice} onChange={handleEditChange} placeholder="Price" />
                  <input name="isbn" value={editData.isbn} onChange={handleEditChange} placeholder="ISBN" />
                  <input name="language" value={editData.language} onChange={handleEditChange} placeholder="Language" />
                  <input name="genre" value={editData.genre} onChange={handleEditChange} placeholder="Genre" />
                  
                  <input type="date" name="startAvailability" value={editData.startAvailability} onChange={handleEditChange} />
                  <input type="date" name="endAvailability" value={editData.endAvailability} onChange={handleEditChange} />
                  <input type="file" name="coverImage" onChange={handleEditChange} accept="image/*" />
                  
                  <div className="edit-buttons">
                    <button onClick={handleSave}>Save</button>
                    <button onClick={handleCancel}>Cancel</button>
                  </div>
                </>
              ) : (
                <>
                  <h3>{post.title}</h3>
                  <p><strong>Author:</strong> {post.bookownername || 'N/A'}</p>
                  <p><strong>Price:</strong> {post.borrowPrice || 'N/A'}</p>
                  <p><strong>Status:</strong> {post.borrowStatus || 'N/A'}</p>
                  <div className="action-buttons">
                    <button onClick={() => handleEditClick(post)}>Edit</button>
                    <button onClick={() => handleDelete(post.id)}>Delete</button>
                  </div>
                </>
              )}
            </div>
          </div>
        ))
      )}
    </div>
  );
};

export default ManagePosts;

