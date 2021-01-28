--[[
    GD50
    Super Mario Bros. Remake

    -- LevelMaker Class --

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

LevelMaker = Class{}

function LevelMaker.generate(width, height)
    local tiles = {}
    local entities = {}
    local objects = {}

    local tileID = TILE_ID_GROUND
    
    -- whether we should draw our tiles with toppers
    local topper = true
    local tileset = math.random(20)
    local topperset = math.random(20)

    -- insert blank tables into tiles for later access
    for x = 1, height do
        table.insert(tiles, {})
    end

    local last = 0 -- BORA.BF     
    keyColor = math.random(4) -- BORA.2 Key Color randomly selected

    -- column by column generation instead of row; sometimes better for platformers
    for x = 1, width do
        local tileID = TILE_ID_EMPTY       
        
        -- lay out the empty space
        for y = 1, 6 do
            table.insert(tiles[y],
                Tile(x, y, tileID, nil, tileset, topperset))
        end

        -- chance to just be emptiness
        -- BORA.1 Avoided chasm in the first 5 and last 10 column
        if (x > 5 and (width - x > 9)) and math.random(7) == 1 then
            for y = 7, height do
                table.insert(tiles[y],
                    Tile(x, y, tileID, nil, tileset, topperset))
            end
        else
            tileID = TILE_ID_GROUND

            -- height at which we would spawn a potential jump block
            local blockHeight = 4

            for y = 7, height do
                table.insert(tiles[y],
                    Tile(x, y, tileID, y == 7 and topper or nil, tileset, topperset))
            end

            -- BORA.BF Having full flat start for the first 5 tiles
            if x < 5 then
                goto tileEnd
            end
            
            -- BORA.BF position the lock at the beggining for the first level
            -- and position the lock at the middle of the level for upper levels
            if width > 20 then
                lockPos = width / 2
            else
                lockPos = width / 4
            end

            -- BORA.2 Lock placed
            if x == lockPos then
                local lock = GameObject {
                    texture = 'keysAndLocks',
                    x = (x - 1) * TILE_SIZE,
                    y = (blockHeight - 1) * TILE_SIZE,
                    width = 16,
                    height = 16,

                    frame = keyColor + 4,
                    collidable = true,
                    solid = true,

                    onCollide = function(player)
                        if Player:GetKey() then                            
                            Player:SetKey(false)
                            -- Remove Dead End sign and the lock block
                            objects.deadEnd = nil
                            objects.lock = nil
                            -- BORA.2 Spawn Flag
                            local flag = GameObject {
                                texture = 'flag',
                                x = (width - 3) * TILE_SIZE,
                                y = 3 * TILE_SIZE,
                                width = 19,
                                height = 48,
                                collidable = false,
                                consumable = true,
                                solid = false,
                                frame = 1,

                                onConsume = function(player)
                                    gSounds['powerup-reveal']:play()
                                    gStateMachine:change('play', {
                                        level = (width / 20) + 1,
                                        score = player.score
                                        })
                                end
                            }
                            table.insert(objects, flag)  
                                                    
                            gSounds['powerup-reveal']:play()
                        else
                            gSounds['empty-block']:play()
                            Player:SetKey(false)
                        end
                    end
                }
                objects.lock = lock
                goto tileEnd
            end

            -- BORA.BF Having Mario-like end which is stairs and post
            -- BORA.2 Key placed
            if width - x <= 9 then
                last = last + 1
                -- Stairs
                if last < 5 then
                    for y = 7 - last, 7 do
                        tiles[y][x] = Tile(x, y, tileID, y == (7 - last) and topper or nil, tileset, topperset)
                    end
                end
                -- Key Placed
                if last == 4 then
                    local key = GameObject {
                        texture = 'keysAndLocks',
                        x = (x - 1) * TILE_SIZE,
                        y = 1 * TILE_SIZE,
                        width = 16,
                        height = 16,
                        collidable = false,
                        consumable = true,
                        solid = false,
                        frame = keyColor,

                        onConsume = function(player)
                            gSounds['pickup']:play()                            
                            Player:SetKey(true, keyColor)             
                        end
                    }
                    table.insert(objects, key)
                end
                -- Dead End Sign
                if width - x == 2 then
                    local deadEnd = GameObject {
                        texture = 'deadEnd',
                        x = (x - 1) * TILE_SIZE,
                        y = 5 * TILE_SIZE,
                        width = 16,
                        height = 15,
                        collidable = false,
                        consumable = false,
                        solid = false,
                        frame = 29
                    }
                    objects.deadEnd = deadEnd   -- BORA.BF Changed way of inserting
                end                             -- due to remove it easily
                goto tileEnd
            end

            -- chance to generate a pillar
            -- BORA.BF pillar avoided in the last 15 column to avoid wierdness
            if width - x > 15 and math.random(5) == 1 then
                blockHeight = 2
                
                -- chance to generate bush on pillar
                if math.random(8) == 1 then
                    table.insert(objects,
                        GameObject {
                            texture = 'bushes',
                            x = (x - 1) * TILE_SIZE,
                            y = (4 - 1) * TILE_SIZE,
                            width = 16,
                            height = 16,
                            
                            -- select random frame from bush_ids whitelist, then random row for variance
                            frame = BUSH_IDS[math.random(#BUSH_IDS)] + (math.random(4) - 1) * 7,
                            collidable = false
                        }
                    )
                end
                
                -- pillar tiles
                tiles[5][x] = Tile(x, 5, tileID, topper, tileset, topperset)
                tiles[6][x] = Tile(x, 6, tileID, nil, tileset, topperset)
                tiles[7][x].topper = nil
            
            -- chance to generate bushes
            elseif math.random(8) == 1 then
                table.insert(objects,
                    GameObject {
                        texture = 'bushes',
                        x = (x - 1) * TILE_SIZE,
                        y = (6 - 1) * TILE_SIZE,
                        width = 16,
                        height = 16,
                        frame = BUSH_IDS[math.random(#BUSH_IDS)] + (math.random(4) - 1) * 7,
                        collidable = false
                    }
                )
            end

            -- chance to spawn a block
            if math.random(13) == 1 then
                table.insert(objects,

                    -- jump block
                    GameObject {
                        texture = 'jump-blocks',
                        x = (x - 1) * TILE_SIZE,
                        y = (blockHeight - 1) * TILE_SIZE,
                        width = 16,
                        height = 16,

                        -- make it a random variant
                        frame = math.random(#JUMP_BLOCKS),
                        collidable = true,
                        hit = false,
                        solid = true,

                        -- collision function takes itself
                        onCollide = function(obj)

                            -- spawn a gem if we haven't already hit the block
                            if not obj.hit then

                                -- chance to spawn gem, not guaranteed
                                if math.random(5) == 1 then

                                    -- maintain reference so we can set it to nil
                                    local gem = GameObject {
                                        texture = 'gems',
                                        x = (x - 1) * TILE_SIZE,
                                        y = (blockHeight - 1) * TILE_SIZE - 4,
                                        width = 16,
                                        height = 16,
                                        frame = math.random(#GEMS),
                                        collidable = true,
                                        consumable = true,
                                        solid = false,

                                        -- gem has its own function to add to the player's score
                                        onConsume = function(player, object)
                                            gSounds['pickup']:play()
                                            player.score = player.score + 100
                                        end
                                    }
                                    
                                    -- make the gem move up from the block and play a sound
                                    Timer.tween(0.1, {
                                        [gem] = {y = (blockHeight - 2) * TILE_SIZE}
                                    })
                                    gSounds['powerup-reveal']:play()

                                    table.insert(objects, gem)
                                end

                                obj.hit = true
                            end

                            gSounds['empty-block']:play()
                        end
                    }
                )
            end

            ::tileEnd:: -- BORA.BF 
        end
    end

    local map = TileMap(width, height)
    map.tiles = tiles
    
    return GameLevel(entities, objects, map)
end