--[[
    GD50
    Legend of Zelda

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

GAME_OBJECT_DEFS = {
    ['switch'] = {
        type = 'switch',
        texture = 'switches',
        frame = 2,
        width = 16,
        height = 16,
        solid = false,
        defaultState = 'unpressed',
        states = {
            ['unpressed'] = {
                frame = 2
            },
            ['pressed'] = {
                frame = 1
            }
        }
    },
    -- BORA.2 define pot
    ['pot'] = {
        type = 'pot',
        texture = 'tiles',
        frame = 14,
        width = 16,
        height = 16,
        solid = true,
        picked = false,
        consumed = false 
    },  
    -- BORA.1 define heart
    ['heart'] = {
        type = 'heart',
        texture = 'hearts',
        frame = 5,
        width = 12,
        height = 12,
        solid = false,
        consumed = false    
    }
}