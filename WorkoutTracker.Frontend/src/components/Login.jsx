import { useState } from 'react'
import { login } from '../services/authService.js'
import { useNavigate } from 'react-router-dom'
import { TextField, Button, Typography, Container, Paper, Box, Alert } from '@mui/material';

const Login = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        setError('');
        try {
            await login(username, password);
            navigate('/dashboard');
        } catch (err) {
            setError(err || 'Failed to Login');
        }
    };

    return (
        <Box 
            sx={{
                height: '100vh',
                width: '100vh',
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                backgroundColor: '#f5f5f5',
            }}
        >
        <Container maxWidth="xs">
        <Paper elevation={3} sx={{ padding: 4, marginTop: 8 }}>
            <Typography variant="h5" align="center" gutterBottom>
            Login
            </Typography>
            {error && <Alert severity="error">{error}</Alert>}
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            <TextField
                label="Username"
                variant="outlined"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                fullWidth
            />
            <TextField
                label="Password"
                type="password"
                variant="outlined"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                fullWidth
            />
            <Button variant="contained" color="primary" onClick={handleLogin}>
                Login
            </Button>
            </Box>
        </Paper>
        </Container>
        </Box>
    );
};

export default Login;