import axios from 'axios'

const API_URL = '/api/auth';

export const login = async (email, password) => {
    try {
        const response = await axios.post(`${API_URL}/login`, { email, password });
        const { token } = response.data;
        localStorage.setItem('token', token);
        return response.data;
    } catch (error) {
        throw error.response?.data || 'Login failed';
    }
};

export const logout = () => {
    localStorage.removeItem('token');
}

export const isAuthenticated= () => {
    return !!localStorage.getItem('token');
};