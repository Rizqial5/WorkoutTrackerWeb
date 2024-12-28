import axios from 'axios'

const API_URL = '/api/workoutplans';

const token = localStorage.getItem('token');

export const getWorkoutPlans = async () => {

    

    try {
        const response = await axios.get(`${API_URL}`,
            {
                headers: {
                    Authorization: `Bearer ${token}`
                },
            });
        return response.data;
    } catch (error) {
        throw error.response?.data || 'Get Plans Failed';
    }
}