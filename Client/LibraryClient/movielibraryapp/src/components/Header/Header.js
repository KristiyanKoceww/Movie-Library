import './Header.css'
import { Link } from 'react-router-dom';
import { useContext, useEffect, useState } from 'react';
import { TextField } from '@mui/material';
import Button from '@mui/material/Button';
import { UserContext } from '../AcountManagment/UserContext';
const Header = () => {
    const { appUser, setAppUser } = useContext(UserContext);

    if (Object.keys(appUser ? appUser : {}).length === 0) {
        return (
            <div className="header">
                <a className="header_title">My Movie Collection</a>
                <div className="login_center">
                    <Link to="/" style={{ textDecoration: 'none' }}>
                        <Button className="home" type="submit" variant="outlined">Home</Button>
                    </Link>
                    {" "}
                    <Link to="/Login" style={{ textDecoration: 'none' }}>
                        <Button className="login" type="submit" variant="outlined">Login</Button>
                    </Link>
                    {" "}
                    <Link to="/Register" style={{ textDecoration: 'none' }}>
                        <Button className="register" type="submit" variant="outlined">Register</Button>
                    </Link>
                </div>
            </div>
        )
    }
    else {
        return (
            <div className="header">
                <a className="header_title">My Movie Collection</a>

                <div className="search_movie">
                    <TextField className="textFieldTitle"
                        label="Search..."
                        color="warning"
                        size="small" id="standard-basic" />
                    <Button className="home" type="submit" variant="outlined">Search</Button>
                </div>


                <div className="login_center">
                    <Link to="/" style={{ textDecoration: 'none' }}>
                        <Button className="home" type="submit" variant="outlined">Home</Button>
                    </Link>
                    {" "}
                    <Link to="/Logout" style={{ textDecoration: 'none' }}>
                        <Button className="login" type="submit" variant="outlined">Logout</Button>
                    </Link>
                </div>
            </div>
        )
    }
}

export default Header