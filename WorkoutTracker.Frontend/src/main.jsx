import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import WorkoutPlans from './WorkoutPlans.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    < WorkoutPlans/>
  </StrictMode>,
)
