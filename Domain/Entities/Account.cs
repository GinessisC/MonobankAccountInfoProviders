namespace Domain.Entities;

public record Account(
	string MaskedPan,
	Money OnBalance);