using AutoMapper;
using ProiectII.DTO.CommentsReport;
using ProiectII.Interfaces;
using ProiectII.Models;
<<<<<<< HEAD

namespace ProiectII.Services.CoreDomain
{
=======
using ProiectII.Repositories;

namespace ProiectII.Services.CoreDomain
{

>>>>>>> origin/master
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

<<<<<<< HEAD
=======

>>>>>>> origin/master
        public async Task<IEnumerable<CommentDto>> GetCommentsByFoxIdAsync(uint foxId)
        {
            var comments = await _repository.GetCommentsByFoxIdAsync(foxId);
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

<<<<<<< HEAD
=======

>>>>>>> origin/master
        public async Task<IEnumerable<CommentDto>> GetCommentsByUserIdAsync(string userId)
        {
            var comments = await _repository.GetCommentsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<CommentDto> CreateCommentAsync(CreateCommentDto dto, string userId)
        {
            var commentEntity = _mapper.Map<Comment>(dto);
<<<<<<< HEAD

            // Setarile care NU vin din DTO
            commentEntity.UserId = userId;
            commentEntity.CreatedAt = DateTime.UtcNow;
            commentEntity.UpdatedAt = null;
            commentEntity.IsDeleted = false;
            commentEntity.IsVisible = true; // Implicit, e vizibil cand e postat

            await _repository.AddAsync(commentEntity);
            await _repository.SaveChangesAsync(); // CRITIC: Executia salvarii

            return _mapper.Map<CommentDto>(commentEntity);
        }

=======
            commentEntity.UserId = userId;
            //commentEntity.IsDeleted = false; // Setăm valoarea implicită pentru soft delete
            commentEntity.CreatedAt = DateTime.UtcNow; // Setăm data curentă pentru createdAt
            commentEntity.UpdatedAt = null;
            commentEntity.IsDeleted = false; // Setăm valoarea implicită pentru soft delete
            commentEntity.Content = dto.Content; // Asigurăm că conținutul este preluat din DTO
            commentEntity.FoxId = dto.FoxId; // Asigurăm că FoxId este preluat din DTO

            await _repository.AddAsync(commentEntity);
            return _mapper.Map<CommentDto>(commentEntity);
        }



>>>>>>> origin/master
        public async Task<bool> EditCommentAsync(uint commentId, UpdateCommentDto dto, string userId)
        {
            var comment = await _repository.GetByIdAsync(commentId);

<<<<<<< HEAD
            if (comment == null || comment.UserId != userId || comment.IsDeleted)
=======
            if (comment == null || comment.UserId != userId)
>>>>>>> origin/master
            {
                return false;
            }

            comment.Content = dto.EditedContent;
            comment.UpdatedAt = DateTime.UtcNow;

<<<<<<< HEAD
            _repository.Update(comment);
            return await _repository.SaveChangesAsync(); // CRITIC: Salvare in DB
        }

=======
            await _repository.UpdateAsync(comment);

            return true;
        }


>>>>>>> origin/master
        public async Task<bool> DeleteCommentAsync(uint commentId, string userId)
        {
            var comment = await _repository.GetByIdAsync(commentId);
            if (comment == null || comment.UserId != userId)
            {
                return false;
            }
<<<<<<< HEAD

            comment.IsDeleted = true;
            comment.UpdatedAt = DateTime.UtcNow;

            _repository.Update(comment);
            return await _repository.SaveChangesAsync();
        }

        // --- FUNCTII ADMIN ---
=======
            // Soft delete!!!
            comment.IsDeleted = true;
            comment.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(comment);
            return true;

        }


        /// <summary>
        ///  fucntii admin!!
        ///  
        /// </summary>
>>>>>>> origin/master

        public async Task<bool> AdminHideComment(uint commentId)
        {
            var comment = await _repository.GetByIdAsync(commentId);
<<<<<<< HEAD
            if (comment == null) return false;

            comment.IsVisible = false;
            comment.UpdatedAt = DateTime.UtcNow;

            _repository.Update(comment);
            return await _repository.SaveChangesAsync();
=======
            if (comment == null)
            {
                return false;
            }
            comment.IsVisible = false;
            comment.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(comment);
            return true;
>>>>>>> origin/master
        }

        public async Task<bool> AdminUnhideComment(uint commentId)
        {
            var comment = await _repository.GetByIdAsync(commentId);
<<<<<<< HEAD
            if (comment == null) return false;

            comment.IsVisible = true;
            comment.UpdatedAt = DateTime.UtcNow;

            _repository.Update(comment);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> AdminDeleteCommentAsync(uint commentId)
        {
            var comment = await _repository.GetByIdAsync(commentId);
            if (comment == null) return false;

            comment.IsDeleted = true;
            comment.UpdatedAt = DateTime.UtcNow;

            _repository.Update(comment);
            return await _repository.SaveChangesAsync();
        }
=======
            if (comment == null)
            {
                return false;
            }
            comment.IsVisible = true;
            comment.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(comment);
            return true;
        }


        public async Task<bool> AdminDeleteCommentAsync(uint commentId)
        {
            var comment = await _repository.GetByIdAsync(commentId);
            if (comment == null)
            {
                return false;
            }
            comment.IsDeleted = true;
            comment.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(comment);
            return true;
        }





>>>>>>> origin/master
    }
}