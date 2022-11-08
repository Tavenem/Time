# Changelog

## 2.1
### Updated
- Update to .NET 7

## 2.1.0-preview.2
### Updated
- Update dependencies

## 2.1.0-preview.1
### Updated
- Update to .NET 7 RC
### Changed
- Epoch list no longer read-only
- `CosmicTime` now uses default JSON (de)serialization
### Removed
- Add/Remove Epoch methods from `CosmicTime`

## 2.0.0
### Updated
- Update to release packages

## 2.0.0-preview.3
### Fixed
- Conversions

## 2.0.0-preview.2
### Changed
- JSON serialization now uses `ToString` round-trip format

## 2.0.0-preview.1
### Changed
- Update to .NET 6 preview
- Update to C# 10 preview
- Remove dependency on `HugeNumber` and use numeric generics
- Use BigInteger for aeons and Planck time
- Changed to readonly struct
### Removed
- Support for non-JSON serialization
- Individual `AddX` and `SubtractX` methods for time units; the `Add(Duration)` method can be used instead after a static `FromX` method

## 1.3.0
### Added
- Add addition and subtraction operator overloads for nullable values

## 1.2.0
### Added
- Add `GetDifferenceFromNow` to `CosmicTime`

## 1.1.0
### Added
- JSON serialization for `CosmicTime`

### Changed
- Changed JSON serialization from string-based to property-based

## 1.0.0
### Added
- Initial release