fn SmoothNormalBake  = (
	local numFaces = getNumFaces $
	local numVerts = getNumVerts $
	local originalSmooth = for i in 1 to numFaces collect getFaceSmoothGroup $ i
	for i in 1 to numFaces do setFaceSmoothGroup $ i 1
	sleep 1
	for i in 1 to numVerts do meshOp.setVertColor $ 0 i ((((getNormal $ i) + 1) * 128) as color )
	for i in 1 to numFaces do setFaceSmoothGroup $ i originalSmooth[i]
)
SmoothNormalBake()

for i in 1 to numVerts do (
	local clr = getNormal $ i
    print (clr.x * clr.x + clr.y * clr.y  + clr.z * clr.z)
)