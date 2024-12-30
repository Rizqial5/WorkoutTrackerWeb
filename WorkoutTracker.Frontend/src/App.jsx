import React from 'react'
import {BrowserRouter as Router, Routes, Route} from 'react-router-dom'
import Login from './components/Login.jsx'
import ProtectedRoute from './components/ProtectedRoute.jsx'
import WorkoutPlans from './components/WorkoutPlans.jsx'
import Sidebar from './components/Sidebar.jsx'
import './App.css'
import theme from './theme/them.js'
import { ThemeProvider } from '@emotion/react'


const Dashboard = () => <h1>Welcome to Dashboard</h1>;
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
                        <Sidebar/>
                        <Dashboard/>
                        <WorkoutPlans/>
                        </ThemeProvider>
                    
                    </ProtectedRoute>
                }/>
        </Routes>
        </Router>
    );

}

export default App
