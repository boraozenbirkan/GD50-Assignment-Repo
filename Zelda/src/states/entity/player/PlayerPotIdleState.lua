PlayerPotIdleState = Class{__includes = EntityIdleState}

function PlayerPotIdleState:enter(params)
    
    -- render offset for spaced character sprite (negated in render function of state)
    self.entity.offsetY = 5
    self.entity.offsetX = 0
    self.pot = params.pot
end

function PlayerPotIdleState:update(dt)
    if love.keyboard.isDown('left') or love.keyboard.isDown('right') or
       love.keyboard.isDown('up') or love.keyboard.isDown('down') then
        self.entity:changeState('pot-walk', {
            pot = self.pot
        })
    end

    self.entity:changeAnimation('potIdle-' .. self.entity.direction)

    if love.keyboard.wasPressed('space') then
        self.pot.moveDirection = self.entity.direction
        self.pot.launchPosX = self.entity.x
        self.pot.launchPosY = self.entity.y
        self.entity:changeState('idle')
    end
end