import { useState } from 'react'
import { login } from '../services/authService.js'
import { useNavigate } from 'react-router-dom'
import { 
    Button, 
    TextField, 
    Card, 
    CardContent, 
    CardActions, 
    Typography, 
    Container, 
    Box,
    ThemeProvider,
    CssBaseline
  } from '@mui/material';
import FitnessCenterIcon from '@mui/icons-material/FitnessCenter';
import theme from '../theme/them.js';




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
        <ThemeProvider theme={theme}>
            <CssBaseline/>
            <Container component="main" maxWidth="xs" sx={{
                display: 'flex',
                justifyContent: 'center'
            }}>
                <Box
                    sx={{
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                    }}
                >
                    <FitnessCenterIcon sx={{m:1, fontSize: 40, color: 'blue'}}/>
                    <Typography component="h1" variant='h5' color='secondary'>
                        FitTrack Pro
                    </Typography>
                    <Typography variant="body2" color="text.secondary" align="center">
                        Login to manage your workout plans
                    </Typography>
                    <Card sx={{ mt: 3, width: '100%', boxShadow: 3 }}>
                        <CardContent>
                        <form onSubmit={handleLogin}>
                            <TextField
                            variant="outlined"
                            margin="normal"
                            required
                            fullWidth
                            id="username"
                            label="Username"
                            name="username"
                            autoComplete="username"
                            autoFocus
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            sx={{ '& .MuiOutlinedInput-root': { '&.Mui-focused fieldset': { borderColor: 'primary.main' } } }}
                            />
                            <TextField
                            variant="outlined"
                            margin="normal"
                            required
                            fullWidth
                            name="password"
                            label="Password"
                            type="password"
                            id="password"
                            autoComplete="current-password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            sx={{ '& .MuiOutlinedInput-root': { '&.Mui-focused fieldset': { borderColor: 'primary.main' } } }}
                            />
                            {error && (
                            <Typography color="error" variant="body2" sx={{ mt: 2 }}>
                                {error}
                            </Typography>
                            )}
                            <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{ mt: 3, mb: 2, bgcolor: 'primary.main', color: 'white', '&:hover': { bgcolor: 'primary.dark' } }}
                            >
                            Sign In
                            </Button>
                        </form>
                        </CardContent>
                        <CardActions>
                        <Typography variant="body2" color="text.secondary" sx={{ margin: 'auto' }}>
                            Don't have an account?{' '}
                            <Button color="primary" size="small" href="#">
                            Sign up
                            </Button>
                        </Typography>
                        </CardActions>
                    </Card>
                </Box>
            </Container>
        </ThemeProvider>
    );
};


export default Login;