--[[
    GD50
    Pokemon

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

MenuState = Class{__includes = BaseState}

function MenuState:init(battleState, pokemon, onClose)
    self.battleState = battleState
    self.pokemon = pokemon
    self.onClose = onClose or function() end
    
    -- Save current values
    self.previousHP = self.pokemon.HP
    self.previousAttack = self.pokemon.attack
    self.previousDefense = self.pokemon.defense
    self.previousSpeed = self.pokemon.speed

    -- Level up
    self.pokemon:levelUp()

    -- Then show before and after values
    self.menu = Menu {
        x = VIRTUAL_WIDTH - 200,
        y = 0,
        width = 200,
        height = VIRTUAL_HEIGHT - 90,
        items = {
            {
                text = 'HP:      ' .. self.previousHP .. ' + ' ..
                    self.pokemon.HP - self.previousHP .. ' = ' .. self.pokemon.HP
            },
            {
                text = 'Attack:  ' .. self.previousAttack .. ' + ' ..
                    self.pokemon.attack - self.previousAttack .. ' = ' .. self.pokemon.attack
            },
            {
                text = 'Defense: ' .. self.previousDefense .. ' + '..
                    self.pokemon.defense - self.previousDefense .. ' = ' .. self.pokemon.defense
            },
            {
                text = 'Speed:   ' .. self.previousSpeed .. ' + ' ..
                    self.pokemon.speed - self.previousSpeed .. ' = ' .. self.pokemon.speed
            }
        },
        enableCursor = false
    }
end

function MenuState:update(dt)
    if love.keyboard.wasPressed('enter') or love.keyboard.wasPressed('return') then
        gStateStack:pop()
        self.onClose()
    end
end

function MenuState:render()
    self.menu:render()
end