import { useEffect, useState } from "react";
import { getWorkoutPlans } from "../services/workoutPlanService";
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from '@mui/material';
import Paper from '@mui/material/Paper';



function WorkoutPlans() {
    const [workoutPlansData, setWorkoutPlansData] = useState([])
    const [loading, setLoading] =  useState(true)
    const [error, setError] = useState('')

    const fetchData = async () => {
        try{
            const data = await getWorkoutPlans();
            setWorkoutPlansData(data);
            setLoading(false);
        } catch(err){
            setError(err.message);
            setLoading(false);
        }
       };
    
    useEffect(() => {
       fetchData();
    }, []);

    if(loading)  return <p>Loading...</p>
    if(error) return <p>Error : {error}</p>

    return (
        <TableContainer component={Paper} >
            <Table sx={{ minWidth: 650 }} aria-label="simple table">
                <TableHead>
                    <TableRow>
                        <TableCell>User</TableCell>
                        <TableCell>Plan Name</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {workoutPlansData.map((wp) => (
                        <TableRow
                            key={wp.planId}
                            sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                        >
                            <TableCell>{wp.user}</TableCell>
                            <TableCell>{ wp.planName}</TableCell>
                            
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    )
}

export default WorkoutPlans