import axios from 'axios'

const API_URL = '/api/auth';

export const login = async (username, password) => {
    try {
        const response = await axios.post(`${API_URL}/login`, { username, password });
        const { token } = response.data;
        localStorage.setItem('token', token);
        localStorage.setItem('username', username);
        return response.data;
    } catch (error) {
        throw error.response?.data || 'Login failed';
    }
};

export const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
}

export const isAuthenticated= () => {
    return !!localStorage.getItem('token');
};