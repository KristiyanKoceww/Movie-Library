import { useState } from 'react';
import { useEffect } from 'react';
import { TextField } from '@mui/material';
import Button from '@mui/material/Button';

import './Search.css'

const SearchMovie = () => {
    const [query, setQuery] = useState('');
    const [movie, setMovie] = useState(null);
    const [isLoading, setIsLoading] = useState(false);

    var searchUrl = process.env.REACT_APP_MOVIEAPIURL;

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
                console.log(result);
                setQuery('')
                setIsLoading(false);
            });
    }
    return (
        <div>
            <div>
                <form onSubmit={Search}>
                    <h1>Search</h1>
                    <div className="search_movie">
                        <TextField className="textFieldTitle"
                            label="Search..."
                            color="warning"
                            size="small" id="standard-basic"
                            onChange={(e) => setQuery(e.target.value)} />
                        {" "}
                        <Button className="home" type="submit" variant="outlined">Search</Button>
                    </div>
                </form>
            </div>
            {
                !isLoading && movie &&
                <div className="movie">
                    <div className="movie_info">
                        <img src={movie.Poster} alt="img" />
                    </div>
                    <div className="movie_info">
                        <h1>{movie.Title} ({movie.Year})</h1>
                        <h5>{movie.Genre} | {movie.Runtime}</h5>
                        <h5>{movie.Plot}</h5>
                        <h5>Awards: {movie.Awards}</h5>
                        <h5>Director: {movie.Director}</h5>
                        <h5>Released: {movie.Released}</h5>
                        <h5>Imdb rating: {movie.imdbRating}</h5>
                        <Button className="home" type="submit" variant="outlined">Add to favourites</Button>
                    </div>
                </div>
            }


        </div>
    )
}

export default SearchMovie