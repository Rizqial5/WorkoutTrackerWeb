import axios from 'axios'

const API_URL = '/api/exercisesets';



export const getExerciseSets = async () => {
    const token = localStorage.getItem('token');
    try {
        const response = await axios.get(`${API_URL}`,
            {
                headers: {
                    Authorization: `Bearer ${token}`
                },
            });
        return response.data;
    } catch (error) {
        throw error.response?.data || 'Get Exercise Sets Failed';
    }
}

export const addExerciseSet = async (exerciseSet) => {
    const token = localStorage.getItem('token');
    try {
        const response = await axios.post(`${API_URL}`, exerciseSet,
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
            });
        return response.status;
    } catch (error) {
        throw error.response?.data || 'Add Exercise Set Failed';
    }
}