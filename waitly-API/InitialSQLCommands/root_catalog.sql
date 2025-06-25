INSERT INTO catalogos (
    Id, 
    IdPadre, 
    Nombre, 
    Descripcion, 
    Nemonico, 
    Valor, 
    Removido, 
    FechaCreacion, 
    IpCreacion
) 
VALUES (
    1,               -- ID específico para facilitar referencias
    1,               -- Se refiere a sí mismo como padre
    'Raíz',          -- Nombre descriptivo
    'Catálogo raíz del sistema', 
    'ROOT',          -- Nemónico único
    0,               -- Valor (pueden ser 0 u otro valor específico)
    0,               -- No está removido (activo)
    NOW(),           -- Fecha actual
    'SYSTEM'         -- Indica que es creado por el sistema
);