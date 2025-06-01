import React, { useState, useEffect } from 'react';
import { FaThumbsUp, FaThumbsDown, FaBook } from 'react-icons/fa';
import * as signalR from '@microsoft/signalr';
import './HomePage.css';

const HomePage = ({ isLoggedIn, onLikeDislike, userRole, onBorrowClick, searchTerm, searchType }) => {
  const [books, setBooks] = useState([]);
  const [connection, setConnection] = useState(null);

  const userId = localStorage.getItem('userId'); // 🟢 تأكد من وجوده

  // 🟢 الاتصال بـ SignalR + تسجيل القارئ
  useEffect(() => {
    if (!userId || userRole !== 'Reader') return;

    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5248/bookhub')
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, [userId, userRole]);

  useEffect(() => {
    if (!connection) return;

    connection
      .start()
      .then(() => {
        console.log('✅ SignalR Connected');
        console.log("Registering:", userId);
        connection.invoke('RegisterReader', userId); // تسجيل القارئ في الجروب

        // استقبال الإشعار
        connection.on('ReceiveNotification', (message) => {
          alert('📬 ' + message); // أو استخدم toast لاحقًا
        });
      })
      .catch(err => console.error('❌ SignalR Error:', err));

    return () => {
      connection.stop();
    };
  }, [connection]);

  // 🔄 تحميل الكتب
  useEffect(() => {
    const fetchBooks = async () => {
      try {
        let url = 'http://localhost:5248/api/bookposts/allposts';

        if (searchTerm.trim() !== '') {
          if (searchType === 'title') {
            url = `http://localhost:5248/api/bookposts/search-by-name/${encodeURIComponent(searchTerm)}`;
          } else if (searchType === 'price') {
            url = `http://localhost:5248/api/bookposts/search-by-price/${encodeURIComponent(searchTerm)}`;
          }
        }

        const response = await fetch(url);
        if (!response.ok) throw new Error('Failed to fetch books');
        const data = await response.json();
        const booksWithDefaults = data.map(book => ({
          ...book,
          likes: book.likes || 0,
          dislikes: book.dislikes || 0,
          comments: book.comments || [],
          userReaction: null,
          commentInput: ''
        }));
        setBooks(booksWithDefaults);
      } catch (error) {
        console.error('Error fetching books:', error);
      }
    };

    fetchBooks();
  }, [searchTerm, searchType]);

  const handleReaction = async (index, reactionType) => {
    if (!isLoggedIn) {
      onLikeDislike();
      return;
    }

    const book = books[index];
    const bookId = book.id;
    const currentReaction = book.userReaction;

    try {
      if (currentReaction === reactionType) {
        await fetch(`http://localhost:5248/api/likes/unlike/${bookId}`, {
          method: 'DELETE',
          headers: { 'Authorization': `Bearer ${localStorage.getItem('accessToken')}` },
        });

        setBooks(prevBooks => prevBooks.map((b, i) => i === index ? {
          ...b,
          likes: reactionType === 'like' ? b.likes - 1 : b.likes,
          dislikes: reactionType === 'dislike' ? b.dislikes - 1 : b.dislikes,
          userReaction: null
        } : b));
      } else {
        await fetch('http://localhost:5248/api/likes', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
          },
          body: JSON.stringify({ bookPostId: bookId, reactionType: reactionType === 'like' }),
        });

        setBooks(prevBooks => prevBooks.map((b, i) => {
          if (i !== index) return b;
          return {
            ...b,
            likes: reactionType === 'like' ? b.likes + 1 : (currentReaction === 'like' ? b.likes - 1 : b.likes),
            dislikes: reactionType === 'dislike' ? b.dislikes + 1 : (currentReaction === 'dislike' ? b.dislikes - 1 : b.dislikes),
            userReaction: reactionType
          };
        }));
      }
    } catch (error) {
      console.error('Error:', error);
    }
  };

  const handleCommentSubmit = async (index) => {
    const book = books[index];
    const commentText = book.commentInput.trim();
    if (!isLoggedIn || !commentText) {
      onLikeDislike();
      return;
    }

    try {
      const response = await fetch('http://localhost:5248/api/comments', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
        },
        body: JSON.stringify({
          bookPostId: book.id,
          text: commentText,
        }),
      });

      if (!response.ok) throw new Error('Failed to post comment');

      const newComment = await response.json();

      setBooks(prevBooks => prevBooks.map((b, i) => i === index ? {
        ...b,
        comments: [...b.comments, newComment],
        commentInput: ''
      } : b));
    } catch (error) {
      console.error('Error posting comment:', error);
    }
  };

  const handleReplySubmit = async (bookIndex, commentId, replyText) => {
    if (!isLoggedIn || !replyText.trim()) {
      onLikeDislike();
      return;
    }

    try {
      const response = await fetch(`http://localhost:5248/api/comments/${commentId}/reply`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
        },
        body: JSON.stringify({ reply: replyText }),
      });

      if (!response.ok) throw new Error('Failed to post reply');

      setBooks(prevBooks => prevBooks.map((book, i) => {
        if (i !== bookIndex) return book;
        return {
          ...book,
          comments: book.comments.map(c =>
            c.id === commentId ? { ...c, reply: replyText } : c
          )
        };
      }));
    } catch (error) {
      console.error('Error posting reply:', error);
    }
  };

  return (
    <div className="home-page-container">
      <div className="cards">
        {books.map((book, bookIndex) => (
          <div className="card" key={book.id}>
            <h3>{book.title}</h3>
            <img src={book.coverImageUrl ? `http://localhost:5248/${book.coverImageUrl}` : 'default-book-image.jpg'} alt={book.title} className="book-image" />
            <p><strong>Author:</strong> {book.bookownername}</p>
            <p><strong>Price:</strong> ${book.borrowPrice}</p>
            <p><strong>Status:</strong> {book.borrowStatus}</p>

            <div className="card-actions">
              <button
                onClick={() => handleReaction(bookIndex, 'like')}
                className={book.userReaction === 'like' ? 'active' : ''}
              >
                <FaThumbsUp /> {book.likes}
              </button>
              <button
                onClick={() => handleReaction(bookIndex, 'dislike')}
                className={book.userReaction === 'dislike' ? 'active' : ''}
              >
                <FaThumbsDown /> {book.dislikes}
              </button>
              {book.borrowStatus !== 'Borrowed' && (
                <button
                  onClick={() => onBorrowClick(book.id)}
                  className="borrow-btn"
                >
                  <FaBook /> Borrow
                </button>
              )}
            </div>

            <div className="comment-section">
              <h4>Comments:</h4>
              {book.comments.map((comment) => (
                <div key={comment.id} className="comment-item">
                  <div className="comment-text">{comment.text}</div>
                  {comment.reply && (
                    <div className="reply-text">Reply: {comment.reply}</div>
                  )}
                  {!comment.reply && isLoggedIn && (
                    <div className="reply-input">
                      <input
                        type="text"
                        placeholder="Write a reply..."
                        onKeyPress={(e) => {
                          if (e.key === 'Enter' && e.target.value.trim()) {
                            handleReplySubmit(bookIndex, comment.id, e.target.value);
                            e.target.value = '';
                          }
                        }}
                      />
                    </div>
                  )}
                </div>
              ))}
              <input
                type="text"
                placeholder="Add a comment..."
                value={book.commentInput || ''}
                onChange={(e) => {
                  const newBooks = [...books];
                  newBooks[bookIndex].commentInput = e.target.value;
                  setBooks(newBooks);
                }}
                onKeyPress={(e) => e.key === 'Enter' && handleCommentSubmit(bookIndex)}
              />
              <button onClick={() => handleCommentSubmit(bookIndex)}>Send</button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default HomePage;






