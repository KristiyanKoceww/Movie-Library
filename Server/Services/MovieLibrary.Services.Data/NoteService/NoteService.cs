namespace MovieLibrary.Services.Data.NoteService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;

    public class NoteService : INoteService
    {
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IDeletableEntityRepository<MovieNote> movieNotesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> appUsersRepository;

        public NoteService(
            IDeletableEntityRepository<Movie> moviesRepository,
            IDeletableEntityRepository<MovieNote> movieNotesRepository,
            IDeletableEntityRepository<ApplicationUser> appUsersRepository)
        {
            this.movieNotesRepository = movieNotesRepository;
            this.moviesRepository = moviesRepository;
            this.appUsersRepository = appUsersRepository;
        }

        public async Task CreateNote(CreateNoteModel createNoteModel)
        {
            var movie = this.moviesRepository.All().Where(x => x.Id == createNoteModel.MovieId).FirstOrDefault();
            var user = this.appUsersRepository.All().Where(x => x.Id == createNoteModel.UserId).FirstOrDefault();

            if (user is null)
            {
                throw new Exception("No user found.");
            }

            if (movie is null)
            {
                throw new Exception("No movie found.");
            }

            var movieNote = new MovieNote()
            {
                Movie = movie,
                MovieId = createNoteModel.MovieId,
                User = user,
                UserId = createNoteModel.UserId,
                Note = createNoteModel.Note,
            };

            movie.MovieNotes.Add(movieNote);

            this.moviesRepository.Update(movie);
            await this.movieNotesRepository.AddAsync(movieNote);

            await this.movieNotesRepository.SaveChangesAsync();
            await this.moviesRepository.SaveChangesAsync();

        }

        public async Task DeleteNote(DeleteMovieNoteModel deleteMovieNoteModel)
        {
            var movie = this.moviesRepository.All().Where(x => x.Id == deleteMovieNoteModel.MovieId).Include(x => x.MovieNotes).FirstOrDefault();
            var note = this.movieNotesRepository.All().Where(x => x.Id == deleteMovieNoteModel.NoteId).FirstOrDefault();

            if (movie is null)
            {
                throw new Exception("No movie found.");
            }

            if (note is not null)
            {
                movie.MovieNotes.Remove(note);
                this.moviesRepository.Update(movie);

                this.movieNotesRepository.HardDelete(note);

                await this.moviesRepository.SaveChangesAsync();
                await this.movieNotesRepository.SaveChangesAsync();
            }
            else
            {
                throw new Exception("No note found.");
            }
        }

        public IEnumerable<MovieNote> GetNotes(GetMovieNoteModel getMovieNoteModel)
        {
            var user = this.appUsersRepository.All().Where(x => x.Id == getMovieNoteModel.UserId).FirstOrDefault();
            var movie = this.moviesRepository.All().Where(x => x.Id == getMovieNoteModel.MovieId).Include(x => x.MovieNotes).FirstOrDefault();
            if (user is null)
            {
                throw new Exception("No user found.");
            }

            if (movie is null)
            {
                throw new Exception("No movie found.");
            }

            var notes = movie.MovieNotes.Where(x => x.UserId == getMovieNoteModel.UserId).ToList();

            if (notes.Count == 0)
            {
                throw new Exception("No movie notes found.");
            }
            else
            {
                return notes;
            }
        }
    }
}
