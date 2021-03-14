--[[
    GD50
    Legend of Zelda

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

GameObject = Class{}

function GameObject:init(def, x, y)
    
    -- string identifying this object type
    self.type = def.type
    self.solid = def.solid

    self.texture = def.texture
    self.frame = def.frame or 1

    -- whether it acts as an obstacle or not
    self.solid = def.solid
    -- BORA.1 whether is consumed or not
    self.consumed = def.consumed

    self.defaultState = def.defaultState
    self.state = self.defaultState
    self.states = def.states

    -- dimensions
    self.x = x
    self.y = y
    self.width = def.width
    self.height = def.height

    -- TEST

    -- default empty collision callback
    self.onCollide = function() end
end

function GameObject:update(player, dt)
    if self.type == 'pot' and self.picked then
        self.x = player.x
        self.y = player.y - 10
    end
end

--BORA.2
function GameObject:collides(target)
    return not (self.x + self.width < target.x or self.x > target.x + target.width or
                self.y + self.height < target.y or self.y > target.y + target.height)
end

function GameObject:render(adjacentOffsetX, adjacentOffsetY)
    -- BORA.1 Modified for powerup
    if self.type == 'heart' then
        love.graphics.draw(gTextures[self.texture], gFrames[self.texture][self.frame],
            self.x + adjacentOffsetX, self.y + adjacentOffsetY)
    -- BORA.2 Added pots
    elseif self.type == 'pot' then
        love.graphics.draw(gTextures[self.texture], gFrames[self.texture][self.frame],
            self.x + adjacentOffsetX, self.y + adjacentOffsetY)
    else
        love.graphics.draw(gTextures[self.texture], gFrames[self.texture][self.states[self.state].frame or self.frame],
            self.x + adjacentOffsetX, self.y + adjacentOffsetY)
    end
end