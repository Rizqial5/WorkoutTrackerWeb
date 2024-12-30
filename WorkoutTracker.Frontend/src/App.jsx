import React, { useState } from 'react'
import {BrowserRouter as Router, Routes, Route} from 'react-router-dom'
import Login from './components/Login.jsx'
import ProtectedRoute from './components/ProtectedRoute.jsx'
import WorkoutPlans from './components/WorkoutPlans.jsx'
import Sidebar from './components/Sidebar.jsx'
import './App.css'
import theme from './theme/them.js'
import { ThemeProvider } from '@emotion/react'
import { AppBar, Box, CssBaseline, IconButton, Toolbar, Typography } from '@mui/material'
import MenuIcon from '@mui/icons-material/Menu';


function Dashboard(){
    const [open, setOpen] = useState(false)
    const toggleDrawer = () =>{
        setOpen(!open);
    }


    return(
        <Box sx={{display: 'flex'}}>
            <AppBar
                position='fixed'
                sx={{
                    width: open ? `calc(100% - ${240}px)` : '100%',
                    ml: open ? `${240}px` : 0,
                    transition: theme.transitions.create(['margin', 'width'], {
                        easing: theme.transitions.easing.sharp,
                        duration: theme.transitions.duration.leavingScreen
                    }),
                }}
            >   
                <Toolbar>
                    <IconButton
                        color='inherit'
                        aria-label='open drawer'
                        onClick={toggleDrawer}
                        edge='start'
                        sx={{ mr: 2, ...(open && { display: 'none' }) }}
                    >
                        <MenuIcon />
                    </IconButton>
                    <Typography variant='h6' noWrap component='div'>
                        Workout Planner
                    </Typography>
                </Toolbar>
            </AppBar>
            <Sidebar openBar={open} toggleDrawer={toggleDrawer}/>
            <Box
                component='main'
                sx={{
                    flexGrow: 1,
                    p: 3,
                    backgroundColor: 'background.default',
                    transition: theme.transitions.create('margin', {
                        easing: theme.transitions.easing.sharp,
                        duration: theme.transitions.duration.leavingScreen,
                    }),
                    marginLeft: `-${240}px`,
                        ...(open && {
                        transition: theme.transitions.create('margin', {
                            easing: theme.transitions.easing.easeOut,
                            duration: theme.transitions.duration.enteringScreen,
                        }),
                        marginLeft: 0,
                    }),
                }}
            >
                <Toolbar/>
                <WorkoutPlans/>
            </Box>
        </Box>
    )
} 
    
function App() {
    return (
        <Router>
        <Routes>
            <Route path="/login" element={<Login  />}/>
            <Route
                path="/dashboard"
                element={
                    <ProtectedRoute>
                        <ThemeProvider theme={theme}>
                        <CssBaseline/>
                            <Dashboard/>
                        </ThemeProvider>
                    </ProtectedRoute>
                }/>
        </Routes>
        </Router>
    );

}

export default App
