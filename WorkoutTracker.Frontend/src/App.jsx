import React from 'react'
import {BrowserRouter as Router, Routes, Route} from 'react-router-dom'
import Login from './components/Login.jsx'
import ProtectedRoute from './components/ProtectedRoute.jsx'
import WorkoutPlans from './components/WorkoutPlans.jsx'
import './App.css'


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
                        <Dashboard/>
                        <WorkoutPlans/>
                    </ProtectedRoute>
                }/>
        </Routes>
        </Router>
    );

}

export default App
