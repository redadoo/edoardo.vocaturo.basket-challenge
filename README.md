# edoardo.vocaturo.basket-challenge

Welcome to the **Basket Challenge** a two week technical game development challenge focused on recreating the core gameplay loop of Miniclip’s mobile game [*Basketball Stars*](https://www.miniclip.com/games/basketball-stars).

This project is being built using **Unity 2021.3.4f1**

## Objective

- Swipe-based shooting mechanics  
- Ball physics and trajectories  
- Basic scoring system:
  - 3 points for perfect shots
  - 2 points for standard shots  
- Backboard bonus:
  - Occasionally the backboard blinks — touching it before scoring grants bonus points (4, 6, or 8)  
- Fireball bonus:
  - Successful shots fill a meter
  - Once full, your ball ignites and you score double points
  - Fireball lasts for a short time or until a missed shot  
- AI-controlled opponent that competes against the player using the same shooting mechanics  
- Custom match mode (Custom difficulty and match duration)  

---

## Game Overview

The game starts with a **main menu** that visually resembles the one in *Basketball Stars*. Currently, only the **"Play"** button is functional.

Upon pressing **Play**, the game displays **three game modes**:

- **Shots** (implemented)
- **Duels** (not implemented)
- **Carrier** (not implemented)

In **Shots** mode, the player competes against an AI-controlled opponent to score as many points as possible within the time limit.

After selecting the **Shots** mode, the player is taken to a **camp selection screen**:

- The **first three fields** are ordered by AI difficulty and match duration:
  - Camp 1: Easy AI
  - Camp 2: Medium AI
  - Camp 3: Hard AI

- The **fourth camp** is a **Custom Camp**, where the player can choose:
  - Match duration
  - Opponent difficulty
---

## To-Do List

- [ ] Clear and refactor existing code  
- [ ] Add audio settings

## Known Issues

- **Sometimes a backboard bonus feedback appears even when the bonus is not active**
- **Shot parable is too high — trajectory appears exaggerated**  
---
