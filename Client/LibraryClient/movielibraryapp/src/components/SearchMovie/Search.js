import { TextField } from '@mui/material';
import Button from '@mui/material/Button';

import { UserContext } from '../AcountManagment/UserContext';
import { useContext, useEffect, useState } from 'react';

import './Search.css'

const SearchMovie = () => {
    const [query, setQuery] = useState('');
    const [movie, setMovie] = useState(null);
    const [isLoading, setIsLoading] = useState(false);

    var searchUrl = process.env.REACT_APP_MOVIEAPIURL;
    var addMovieToFav = process.env.REACT_APP_BASEURL + 'api/Movie/AddToFavourites';
    const jwt = localStorage.getItem("jwt");
    const { appUser, setAppUser } = useContext(UserContext);

    const Search = async (e) => {
        e.preventDefault();
        setIsLoading(true);

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
                setIsLoading(false);
            });
    }

    const AddToFav = async(e, movie) =>{
        e.preventDefault();
        setIsLoading(true);

        const movieLenght = movie.Runtime.replace(/\D/g, '');
      
        const data = {
            Title: movie.Title,
            Description: movie.Plot,
            Year: movie.Year,
            Lenght: movieLenght,
            ImageUrl: movie.Poster,
            UserId: appUser.id
        }

        fetch(addMovieToFav, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Authorization': 'Bearer ' + jwt
            },
            body: JSON.stringify(data)
        }
        ).then(r => {
            if (!r.ok) {
                throw new Error('Adding to favourites failed!');
            }
            return r.json()
        })
            .then(result => {
                setIsLoading(false);
            });
    }

    return (
        <div>
            <div>
                <form onSubmit={Search}>
                    <h1>Search</h1>
                    <br />
                    <div className="search_movie">
                        <TextField className="textFieldTitle"
                            label="Search..."
                            size="small" id="standard-basic"
                            onChange={(e) => setQuery(e.target.value)}
                            required />
                        {" "}
                        <Button className="home" type="submit" variant="outlined">Search</Button>
                    </div>
                </form>
            </div>
            {
                movie &&
                <div className="movie">
                    <div className="movie_poster">
                        <img src={movie.Poster} alt="img" />
                    </div>
                    <div className="movie_info">
                        <h1>{movie.Title} ({movie.Year})</h1>
                        <h3>{movie.Genre} | {movie.Runtime}</h3>
                    </div>
                    <div className="movie_info">
                        <p className="movie_plot">{movie.Plot}</p>
                    </div>
                    <div className="movie_info">
                        <p>Awards: {movie.Awards}</p>
                        <p>Director: {movie.Director}</p>
                        <p>Released: {movie.Released}</p>
                        <p>Imdb rating: {movie.imdbRating}</p>
                        <div className="btn_fav">
                            <Button className="home" size='large' variant="outlined" onClick={(e) => AddToFav(e, movie)}>Add to favourites</Button>
                        </div>
                    </div>
                </div>

            }


        </div >
    )
}

export default SearchMovie