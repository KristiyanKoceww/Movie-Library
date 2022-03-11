import './Hero.css'
import Button from "@mui/material/Button";
import { Link } from 'react-router-dom';

import { useContext } from 'react';
import { UserContext } from '../AcountManagment/UserContext';
const Hero = () => {
    const { appUser, setAppUser } = useContext(UserContext);

    if (Object.keys(appUser ? appUser : {}).length === 0) {
        return (
            <div className="hero">
                <h1 className="hero_title">Find your favourites movies!</h1>
                <p className="hero_description">Browse through hundreds of movies. Add them to your favourites and rate them.</p>
            </div>
        )
    }
    else {
        return (
            <div className="hero">
                <h1 className="hero_title">Find your favourites movies!</h1>
                <p className="hero_description">Browse through hundreds of movies. Add them to your favourites and rate them.</p>
                <div className="search_b">
                    <Link to="/Search" style={{ textDecoration: 'none' }}>
                        <Button className="search_button" type="submit" variant="outlined">
                            Search
                        </Button>
                    </Link>
                </div>
            </div>
        )
    }
}

export default Hero