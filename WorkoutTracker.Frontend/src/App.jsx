import React from 'react'
import {BrowserRouter as Router, Routes, Route} from 'react-router-dom'
import Login from '@/components/Login'
import ProtectedRoute from '@/components/ProtectedRoute'


const Dashboard = () => <h1>Welcome to Dashboard</h1>;
function App() {

    return (
        <Router>
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route
                    path="/dashboard"
                    element={
                        <ProtectedRoute>
                            <Dashboard/>
                        </ProtectedRoute>
                    }

                />

            </Routes>
        </Router>
    )
  
}

export default App
