# Changelog

## 0.3.1 - 2026-09-25

### Changed

- Reduced repeated reflection work during craft hint redraws.
- Added caching for Queue Count UI references while preserving pooled widget behavior.
- Reduced unnecessary string formatting and visibility updates when the displayed queue count has not changed.
- Improved runtime efficiency without changing queue-count behavior.
- Release builds now use the optimized Release configuration.

## 0.3.0

- Added support for mixed crafting queues.
- The counter now only counts crafts matching the currently displayed recipe.
- Improved queue count behavior while crafting.
- The vanilla output counter remains unchanged.

## 0.2.0

- Initial release.
- Added an in-world counter showing the remaining number of crafts.
- Integrated the counter into the existing crafting indicator.
- Counter updates automatically as crafts are completed.
- Uses the game's existing UI style.
