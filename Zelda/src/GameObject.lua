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

    -- default empty collision callback
    self.onCollide = function() end
end

function GameObject:update(room, player, dt)
    if self.type == 'pot' and self.picked then
        -- if we got a move direction, that means we have been launched
        if self.moveDirection == nil then
            self.x = player.x
            self.y = player.y - 10
        else
            if self.moveDirection == 'left' then
                self.x = self.x - POT_SPEED * dt
            elseif self.moveDirection == 'right' then
                self.x = self.x + POT_SPEED * dt
            elseif self.moveDirection == 'down' then
                self.y = self.y + POT_SPEED * dt
            elseif self.moveDirection == 'up' then
                self.y = self.y - POT_SPEED * dt
            end

            -- Disappear after travel 4 tiles
            if (self.x > self.launchPosX + TILE_SIZE * 4 or self.x < self.launchPosX - TILE_SIZE * 4 or
            self.y > self.launchPosY + TILE_SIZE * 4 or self.y < self.launchPosY - TILE_SIZE * 4 ) then
                self.consumed = true
            end

            -- Kill if collide with an enemy and disappear
            if not self.consumed then
                for i = #room.entities, 1, -1 do
                    local entity = room.entities[i]
    
                    if self:collides(entity) then
                        entity:damage(1)
                        self.consumed = true
                    end
                end
            end

            -- Disappear if collide with walls (left, right, up, down)
            if self.moveDirection == 'left' then
                if self.x <= MAP_RENDER_OFFSET_X + TILE_SIZE then 
                    self.consumed = true
                end
            elseif self.moveDirection == 'right' then
                if self.x + self.width >= VIRTUAL_WIDTH - TILE_SIZE * 2 then
                    self.consumed = true
                end
            elseif self.moveDirection == 'up' then
                if self.y <= MAP_RENDER_OFFSET_Y + TILE_SIZE - self.height / 2 then 
                    self.consumed = true
                end
            elseif self.moveDirection == 'down' then
                local bottomEdge = VIRTUAL_HEIGHT - (VIRTUAL_HEIGHT - MAP_HEIGHT * TILE_SIZE) 
                    + MAP_RENDER_OFFSET_Y - TILE_SIZE
        
                if self.y + self.height >= bottomEdge then
                    self.consumed = true
                end
            end
        end
        
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