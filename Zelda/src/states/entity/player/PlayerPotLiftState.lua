


PlayerPotLiftState = Class{__includes = EntityIdleState}

function PlayerPotLiftState:init(player)
    self.player = player
    
    -- render offset for spaced character sprite
    self.player.offsetY = 5
    self.player.offsetX = 8

    local direction = self.player.direction

    self.player:changeAnimation('potLift-' .. self.player.direction)

    self.pot = pot or nil
end

function PlayerPotLiftState:enter(params)    
    self.player.currentAnimation:refresh()
    self.pot = params.pot
end

function PlayerPotLiftState:update(dt)

    if self.player.currentAnimation.timesPlayed > 0 then
        self.player.currentAnimation.timesPlayed = 0
        self.player:changeState('pot-idle')
        self.pot.picked = true
    end

end

function PlayerPotLiftState:render()
    local anim = self.player.currentAnimation
    love.graphics.draw(gTextures[anim.texture], gFrames[anim.texture][anim:getCurrentFrame()],
        math.floor(self.player.x - self.player.offsetX), math.floor(self.player.y - self.player.offsetY))

    --
    -- debug for player and hurtbox collision rects VV
    --
    --[[
    love.graphics.setColor(255, 0, 255, 255)
    love.graphics.rectangle('line', self.player.x, self.player.y, self.player.width, self.player.height)
    love.graphics.rectangle('line', self.swordHitbox.x, self.swordHitbox.y,
        self.swordHitbox.width, self.swordHitbox.height)
    love.graphics.setColor(255, 255, 255, 255)
    ]]--
end