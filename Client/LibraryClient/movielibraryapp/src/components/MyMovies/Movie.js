import { UserContext } from '../AcountManagment/UserContext';
import { useContext, useEffect, useState } from 'react';
import { Button } from '@mui/material';
import { Rating } from 'react-simple-star-rating';
import './Movie.css'


const Movie = (movie) => {
    const { appUser, setAppUser } = useContext(UserContext);
    const [rating, setRating] = useState(0)
    const [note, setNote] = useState(null);
    const [userNotes, setUserNotes] = useState();
    const jwt = localStorage.getItem("jwt");

    useEffect(() => {
        const movieId = appUser.movies.find(obj => { return obj.title === movie.movie.Title }) ?
            appUser.movies.find(obj => { return obj.title === movie.movie.Title }).id : null;

        if (movieId) {
            const getMovieNotesUrl = process.env.REACT_APP_BASEURL + 'api/MovieNotes/GetMovieNote?movieId=' + movieId + '&userId=' + appUser.id;
            fetch(getMovieNotesUrl, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    'Authorization': 'Bearer ' + jwt
                },
            }
            ).then(r => {
                if (!r.ok) {
                    throw new Error("Fetching failed!");
                }
                return r.json();
            }).then(r => {
                setUserNotes(r);
            })
        }
    }, []);

    const handleNotes = (e) => {
        e.preventDefault();
        var addMovieNoteUrl = process.env.REACT_APP_BASEURL + 'api/MovieNotes/AddNote';
        const movieId = appUser.movies.find(obj => { return obj.title === movie.movie.Title }).id;
        const data = {
            Note: note,
            MovieId: movieId,
            UserId: appUser.id,
        }
        fetch(addMovieNoteUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: "Bearer " + jwt,
            },
            body: JSON.stringify(data),
        })
            .then(res => {
                if (!res.ok) {
                    throw new Error('Failed to set the note!');
                }
            })
    }
    const handleRating = (rate) => {
        switch (rate) {
            case rate = 0: setRating(1)
                break;
            case rate = 20: setRating(2)
                break;
            case rate = 40: setRating(3)
                break;
            case rate = 60: setRating(4)
                break;
            case rate = 80: setRating(5)
                break;
            default:
                break;
        }

        var setMovieRateUrl = process.env.REACT_APP_BASEURL + 'api/Votes/AddVote';
        const movieId = appUser.movies.find(obj => { return obj.title === movie.movie.Title }).id;
        const data = {
            movieId: movieId,
            userId: appUser.id,
            value: rating,
        }
        fetch(setMovieRateUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: "Bearer " + jwt,
            },
            body: JSON.stringify(data),
        })
            .then(res => {
                if (!res.ok) {
                    throw new Error('Failed to set the vote!');
                }
            })
    }
    const handleRemove = (e) =>{
        e.preventDefault();

        var removeMovieFromFav = process.env.REACT_APP_BASEURL + 'api/Movie/RemoveFromFavourites';
        const movieId = appUser.movies.find(obj => { return obj.title === movie.movie.Title }).id;
        const data = {
            UserId: appUser.id,
            MovieId: movieId,
        }
        fetch(removeMovieFromFav, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: "Bearer " + jwt,
            },
            body: JSON.stringify(data),
        })
            .then(res => {
                if (!res.ok) {
                    throw new Error('Failed to remove the movie!');
                }
            })
    }
    return (
        <div>
            <div className="movie">
                <div className="movie_poster">
                    <img src={movie.movie.Poster} alt="img" />
                </div>
                <div className="movie_info">
                    <h1>{movie.movie.Title} ({movie.movie.Year})</h1>
                    <h3>{movie.movie.Genre} | {movie.movie.Runtime}</h3>
                </div>
                <div className="movie_info">
                    <p className="movie_plot">{movie.movie.Plot}</p>
                </div>
                <div className="movie_info">
                    <p>Awards: {movie.movie.Awards}</p>
                    <p>Director: {movie.movie.Director}</p>
                    <p>Released: {movie.movie.Released}</p>
                    <p>Imdb rating: {movie.movie.imdbRating}</p>
                </div>
                <div className="movie_info">
                    {appUser && appUser.movies.find(obj => { return obj.title === movie.movie.Title }) &&
                        <div>
                            <Button className="removeFromFav" type="submit" variant="outlined" onClick={handleRemove}>Remove from favourites</Button>
                        </div>
                    }
                </div>

            </div>
            <div className="rating">
                {appUser && appUser.movies.find(obj => { return obj.title === movie.movie.Title }) &&
                    <div>
                        Your Review
                        <div>
                            <Rating onClick={handleRating} ratingValue={rating} />
                        </div>
                    </div>
                }
            </div>
            <div className="notes">
                {appUser && appUser.movies.find(obj => { return obj.title === movie.movie.Title }) &&
                    <div>
                        <div>
                            Add Notes
                        </div>
                        <br />
                        <div className="movie_note">
                            <textarea onChange={(e) => setNote(e.target.value)}>

                            </textarea>
                            <div className="save_button">
                                <Button className="save_note_button" type="submit" variant="outlined" onClick={handleNotes}>Save</Button>
                            </div>
                        </div>
                    </div>
                }
            </div>
            <div className="usersnotes">
                <div>
                    {userNotes && <h1 className="notes_title">Your notes:</h1> }
                    {
                        userNotes && userNotes.map((item, idex) => {
                            return (
                                <div className="user_notes">
                                    {item.note}
                                </div>
                            )
                        })

                    }
                </div>
            </div>
        </div>
    )
}

export default Movie