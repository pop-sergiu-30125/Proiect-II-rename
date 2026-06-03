using AutoMapper;
using ProiectII.DTO.FoxManagement;
using ProiectII.Interfaces;
using ProiectII.Models;
using ProiectII.Repositories;

namespace ProiectII.Services.CoreDomain;

public class FoxService(
    IFoxRepository foxRepository,
    ILocationRepository locationRepository,
    IFileStorageService fileStorageService,
    IMapper mapper) : IFoxService
{
    public async Task<IEnumerable<FoxSummaryDto>> GetAllFoxesAsync()
    {
        var foxes = await foxRepository.GetFoxesWithDetailsAsync();
        var activeFoxes = foxes.Where(f => !f.IsDeleted);
        return mapper.Map<IEnumerable<FoxSummaryDto>>(activeFoxes);
    }

    public async Task<FoxDetailsDto?> GetFoxByIdAsync(uint id)
    {
        var fox = await foxRepository.GetFoxByIdWithDetailsAsync(id);
        if (fox == null || fox.IsDeleted) return null;

        return mapper.Map<FoxDetailsDto>(fox);
    }

    public async Task<FoxDetailsDto> CreateFoxAsync(CreateFoxDto dto)
    {
        string? savedImageUrl = null;
        if (dto.Image != null && dto.Image.Length > 0)
        {
            savedImageUrl = await fileStorageService.SaveFileAsync(dto.Image, "foxes");
        }

        var newLocation = new Location
        {
            Name = $"Initial sighting: {dto.Name}",
            Coordinate = new Coordinate
            {
                Latitude = (decimal)dto.FirstSeenLatitude,
                Longitude = (decimal)dto.FirstSeenLongitude
            }
        };

        await locationRepository.AddAsync(newLocation);
        await locationRepository.SaveChangesAsync();

        var newFox = new Fox
        {
            Name = dto.Name,
            Description = dto.Description,
            ImageUrl = savedImageUrl ?? string.Empty,
            StatusId = dto.StatusId,
            FirstSeenLocationId = newLocation.Id,
            LastSeenLocationId = newLocation.Id, // Set initial last seen as first seen
            IsDeleted = false
        };

        await foxRepository.AddAsync(newFox);
        await foxRepository.SaveChangesAsync();

        // Reload to get navigation properties for mapping
        var savedFox = await foxRepository.GetFoxByIdWithDetailsAsync(newFox.Id);
        return mapper.Map<FoxDetailsDto>(savedFox);
    }

    public async Task<bool> UpdateFoxAsync(uint foxId, UpdateFoxDto dto)
    {
        var fox = await foxRepository.GetFoxByIdWithDetailsAsync(foxId);
        if (fox == null || fox.IsDeleted) return false;

        fox.Name = dto.Name;
        fox.Description = dto.Description;
        fox.StatusId = dto.StatusId;
        
        // Handle enclosure - if 0 or null, set to null in DB
        fox.EnclosureId = (dto.EnclosureId == 0) ? null : dto.EnclosureId;

        // Note: Location updates are typically handled by UpdateFoxLocation endpoint,
        // but we handle them here as well if provided in the DTO.
        if (dto.Latitude.HasValue && dto.Longitude.HasValue)
        {
            if (fox.FirstSeenLocation != null)
            {
                fox.FirstSeenLocation.Coordinate.Latitude = dto.Latitude.Value;
                fox.FirstSeenLocation.Coordinate.Longitude = dto.Longitude.Value;
            }
        }

        foxRepository.Update(fox);
        return await foxRepository.SaveChangesAsync();
    }

    public async Task<bool> UpdateFoxStatusAsync(uint foxId, UpdateFoxStatusDto dto)
    {
        var fox = await foxRepository.GetByIdAsync(foxId);
        if (fox == null || fox.IsDeleted) return false;

        fox.StatusId = dto.NewStatusId;

        foxRepository.Update(fox);
        return await foxRepository.SaveChangesAsync();
    }

    public async Task<bool> ArchiveFoxAsync(uint foxId)
    {
        var fox = await foxRepository.GetByIdAsync(foxId);
        if (fox == null) return false;

        fox.IsDeleted = true;

        foxRepository.Update(fox);
        return await foxRepository.SaveChangesAsync();
    }


    public async Task<IEnumerable<FoxMapMarkerDto>> GetMapMarkersAsync()
    {
        var foxes = await foxRepository.GetFoxesWithDetailsAsync();

        var markers = mapper.Map<IEnumerable<FoxMapMarkerDto>>(foxes);

        return markers.Where(m => m.Latitude != 0 && m.Longitude != 0);
    }

}