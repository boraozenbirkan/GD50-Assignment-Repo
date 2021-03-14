--[[
    GD50
    Legend of Zelda

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

Player = Class{__includes = Entity}

function Player:init(def)
    Entity.init(self, def)
end

function Player:update(dt)
    Entity.update(self, dt)
end

function Player:collides(target)
    local selfY, selfHeight = self.y + self.height / 2, self.height - self.height / 2
    
    return not (self.x + self.width < target.x or self.x > target.x + target.width or
                selfY + selfHeight < target.y or selfY > target.y + target.height)
end

-- BORA.2
function Player:potCollision(dt)
    if self.direction == 'left' then
        self.x = self.x + PLAYER_WALK_SPEED * dt
    elseif self.direction == 'right' then
        self.x = self.x - PLAYER_WALK_SPEED * dt
    elseif self.direction == 'up' then
        self.y = self.y + PLAYER_WALK_SPEED * dt
    else 
        self.y = self.y - PLAYER_WALK_SPEED * dt
    end
end

function Player:render()
    Entity.render(self)
    
    love.graphics.setColor(255, 0, 255, 255)
    love.graphics.rectangle('line', self.x, self.y, self.width, self.height)
    love.graphics.setColor(255, 255, 255, 255)
end