PlayerPotIdleState = Class{__includes = EntityIdleState}

function PlayerPotIdleState:enter(params)
    
    -- render offset for spaced character sprite (negated in render function of state)
    self.entity.offsetY = 5
    self.entity.offsetX = 0
end

function PlayerPotIdleState:update(dt)
    if love.keyboard.isDown('left') or love.keyboard.isDown('right') or
       love.keyboard.isDown('up') or love.keyboard.isDown('down') then
        self.entity:changeState('pot-walk')
    end

    if self.entity.direction == 'left' then
        self.entity:changeAnimation('potIdle-left')
    elseif self.entity.direction == 'right' then
        self.entity:changeAnimation('potIdle-right')
    elseif self.entity.direction == 'down' then
        self.entity:changeAnimation('potIdle-down')
    else
        self.entity:changeAnimation('potIdle-up')
    end
end