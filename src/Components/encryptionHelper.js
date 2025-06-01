import CryptoJS from 'crypto-js';

const SECRET_KEY = 'your-secret-key-here'; // يجب أن يطابق مفتاح الخادم

export const decrypt = (encryptedText) => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedText, SECRET_KEY);
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (error) {
    console.error('Decryption error:', error);
    return 'Decryption failed';
  }
};