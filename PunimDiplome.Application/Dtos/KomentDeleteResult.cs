namespace PunimDiplome.Application.Dtos;

public enum KomentDeleteStatus { Deleted, NotFound, Forbidden }

public record KomentDeleteResult(KomentDeleteStatus Status, int? ProduktId);