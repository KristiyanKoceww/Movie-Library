import './MyMovies.css'
import { useContext, useEffect, useState } from 'react';
import { UserContext } from '../AcountManagment/UserContext';
import { Link } from 'react-router-dom';
const MyMovies = () => {
    const { appUser, setAppUser } = useContext(UserContext);
    const [movies, setMovies] = useState([]);
    const [error, setError] = useState();
    const jwt = localStorage.getItem("jwt");


    useEffect(() => {
        if (!(Object.keys(appUser ? appUser : {}).length === 0)) {
            var getUserFavMoviesUrl = process.env.REACT_APP_BASEURL + 'api/Movie/GetFavourites?userId=' + appUser.id;
            fetch(getUserFavMoviesUrl,
                {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json",
                        'Authorization': 'Bearer ' + jwt
                    },
                })
                .then(r => {
                    if (!r.ok) {
                        throw new Error('Getting favourites movies failed!')
                    }
                    return r.json();
                })
                .then(result => {
                    setMovies(result)
                })
                .catch(err => setError(err.message))
        }
    }, [appUser]);

    if (Object.keys(appUser ? appUser : {}).length === 0) {
        return (
            null
        )
    }
    else {
        return (
            <div className="favourites">
                <h1 className="title">Your Favorites</h1>
                {movies.map((item, index) => {
                    return (
                        <img keys={index} src={item.imageUrl} style={{ padding: 10 }} />
                    )
                })}

            </div>
        )
    }
}

export default MyMovies