# WorstBracketBingo

A very basic reverse-bracket app for playing bingo with any fantasy draft style tournament brackets to make picking losing teams more fun. You can create multiple bingo boards for each bracket for you and your friends to see who has the worst(best) picks. For each bracket, stats tracked include: first bingo, last bingo, total rounds alive for all entrants (per bingo board), first complete elimination (all board pieces eliminated), and the last entrants alive for the tournament (either winner or the last one(s) eliminated).

This app is still in the very early stages, it currently works best for small groups of people (2-10) and doesn't accomodate large numbers of boards. In the current build it is best for one person to handle updates.

Installation:

requires .Net Core 2.0: https://www.microsoft.com/net/download/core

detailed OS installation directions: https://docs.microsoft.com/en-us/aspnet/core/publishing/?tabs=aspnetcore2x

Getting Started:
1. On the entrants page, create an entry for each entrant on your bingo board (25 in total) if they aren't already there.
2. Create bracket with any optional external links to the tournament bracket.
3. On the manage page click add board and select all 25 entrants for your board, with the number 1 pick always being the center piece.
4. Click begin round, while the first round is still pending results it is still possible to add more boards
5. Eliminate any entrants as they fall from the bracket in the current round, pending eliminations will be greyed out on the results page.
6. Click to next round on the manage page to begin the next round, or finish bracket once 0 or 1 entrants are left.
7. The results page will update stats and bingo boards upon completion of each round.
