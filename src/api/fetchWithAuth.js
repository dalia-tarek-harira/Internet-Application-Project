export const fetchWithAuth = async (url, options = {}) => {
  let accessToken = localStorage.getItem('accessToken');
  let refreshToken = localStorage.getItem('refreshToken');

  const defaultHeaders = {
    'Content-Type': 'application/json',
    ...(accessToken && { 'Authorization': `Bearer ${accessToken}` }),
  };

  const updatedOptions = {
    ...options,
    headers: {
      ...defaultHeaders,
      ...(options.headers || {}),
    }
  };

  let response = await fetch(url, updatedOptions);

  if (response.status === 401 && !options._retry) {
    // Try refresh
    try {
      const refreshResponse = await fetch('http://localhost:5248/api/account/refresh-token', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(refreshToken),
      });

      if (!refreshResponse.ok) {
        throw new Error('Refresh token failed');
      }

      const data = await refreshResponse.json();
      const newAccessToken = data.accessToken;
      const newRefreshToken = data.refreshToken;

      localStorage.setItem('accessToken', newAccessToken);
      localStorage.setItem('refreshToken', newRefreshToken);

      // Retry original request
      const retryOptions = {
        ...options,
        _retry: true, // Prevent infinite loop
        headers: {
          ...options.headers,
          'Authorization': `Bearer ${newAccessToken}`
        }
      };

      return fetch(url, retryOptions);

    } catch (err) {
      console.error('Refresh token expired. Redirecting to login...');
      localStorage.clear();
      window.location.href = '/login';
      throw err;
    }
  }

  return response;
};
export default fetchWithAuth;
