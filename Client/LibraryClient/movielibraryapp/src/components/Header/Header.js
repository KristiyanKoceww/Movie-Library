import './Header.css'
import { Link } from 'react-router-dom';
import { useContext, useEffect, useState } from 'react';
import { TextField } from '@mui/material';
import Button from '@mui/material/Button';
import { UserContext } from '../AcountManagment/UserContext';
import { Redirect } from 'react-router-dom';
import { useHistory } from 'react-router-dom';
const Header = () => {
    const { appUser, setAppUser } = useContext(UserContext);
    const [query, setQuery] = useState('');
    const [movie, setMovie] = useState(null);
    const [redirect, setRedirect] = useState(false);

    const history = useHistory();

    const search = async (e) => {
        var searchUrl = process.env.REACT_APP_MOVIEAPIURL;
        e.preventDefault();

        fetch(searchUrl + query + '&?plot=full', {
            method: "GET",
        }
        ).then(r => {
            if (!r.ok) {
                throw new Error('Fetching data failed!');
            }
            return r.json()
        })
            .then(result => {
                setMovie(result);

                setRedirect(true);
            })
    }

    if (redirect === true) {
        const location = {
            pathname: "/movies/" + movie.Title,
            state: { referrer: movie }
        }
        history.push(location);
    }

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
                    {/* <TextField className="textFieldTitle"
                        label="Search..."
                        size="small" id="standard-basic"
                        onChange={(e) => setQuery(e.target.value)} /> */}
                    <input required onChange={(e) => setQuery(e.target.value)} />
                    {" "}
                    <Button className="home" type="submit" variant="outlined" onClick={(e) => search(e)}>Search</Button>
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