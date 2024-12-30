import React, { useState } from 'react'
import {BrowserRouter as Router, Routes, Route} from 'react-router-dom'
import Login from './components/Login.jsx'
import ProtectedRoute from './components/ProtectedRoute.jsx'
import WorkoutPlans from './components/WorkoutPlans.jsx'
import Sidebar from './components/Sidebar.jsx'
import './App.css'
import theme from './theme/them.js'
import { ThemeProvider } from '@emotion/react'
import { CssBaseline, Box} from '@mui/material'
import Dashboard from './components/Dashboard.jsx'
import TopBar from './components/TopBar.jsx'
import styled from "styled-components";



const Page = styled.div`
  width: 100vw;
  height: 100%;
  min-height: 100%;
  box-sizing: border-box;
  overflow: hidden;
  display: flex;
`;



function App() {

    const [open, setOpen] = useState(false)
    const toggleDrawer = () =>{
        setOpen(!open);
    }


    return (
        <Page>
        <Router>
        <Routes>
            <Route path="/login" element={<Login  />}/>
            <Route
                path="/dashboard"
                element={
                    <ProtectedRoute>
                        <ThemeProvider theme={theme}>
                        <CssBaseline/>
                        <Box sx={{display: 'flex'}}>
                            <TopBar open={open} toggleDrawer={toggleDrawer}/>
                            <Sidebar openBar={open} toggleDrawer={toggleDrawer} />
                            <Dashboard open={open}/>
                        </Box>
                        </ThemeProvider>
                    </ProtectedRoute>
                }/>
        </Routes>
        </Router>
        </Page>
    );

}

export default App
