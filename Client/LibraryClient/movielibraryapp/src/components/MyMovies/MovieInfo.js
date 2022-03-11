import Movie from './Movie'
import { useHistory } from 'react-router-dom';
import Header from '../Header/Header';

const MovieInfo = () => {
    const history = useHistory();
    const movie = history.location.state.referrer;

    return (
        <div>
            <Header />
            <Movie movie={movie} />
        </div>

    )
}

export default MovieInfo